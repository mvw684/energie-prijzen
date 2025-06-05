// Copyright (c) 2025 mvw684

using System;
using System.IO;

using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Apparaten {
    internal class SlimmeMeter {
        private const string dateTimeFormat = "dd-MM-yyyy HH:mm:ss K";
        private static readonly string[] dateTimeFormats = [dateTimeFormat];

        private readonly InputData inputData;
        public SlimmeMeter(InputData inputData) => this.inputData = inputData;

        public bool Load(out TimeStampedDataList<MeterData> meterData) {
            Tracer.Trace("Loading slimme meter data");
            meterData = new TimeStampedDataList<MeterData>();
            var meterFolder = new DirectoryInfo(inputData.Settings.SlimmeMeter);
            bool result = ReadMeterData(meterFolder, meterData);
            Tracer.Trace("Loading slimme meter data" + (result ? "succeeded" : "failed"));
            return result;
        }

        private bool ReadMeterData(
            DirectoryInfo meterFolder, 
            TimeStampedDataList<MeterData> meterData
        ) {
            bool result = true;
            var gasData = new TimeStampedDataList<GasData>();
            var stroomData = new TimeStampedDataList<StroomData>();
            foreach (FileInfo fileInfo in meterFolder.EnumerateFiles("*.xlsx", SearchOption.TopDirectoryOnly)) {
                try {
                    using (var reader = new ExcelReader(fileInfo)) {
                        string[] header = reader.Header;
                        GC.KeepAlive(header);
                        if (header.Length == 3) {
                            Tracer.Trace($"Reading gas verbruik {fileInfo.FullName}: {string.Join(", ", header)}");
                            result &= ReadGasverbruik(reader, gasData);
                        } else if (header.Length == 6) {
                            Tracer.Trace($"Reading stroom verbruik {fileInfo.FullName}: {string.Join(", ", header)}");
                            result &= ReadStroomVerbruik(reader, stroomData);
                        } else {
                            Tracer.Trace($"Skipping {fileInfo.FullName}: {string.Join(", ", header)}");
                            result = false;
                        }
                    }
                } catch (Exception ex) {
                    Tracer.Trace($"Error reading file {fileInfo.FullName}: {ex.Message}");
                    result = false;
                }
            }
            result &= gasData.Aggregate();
            result &= stroomData.Aggregate();
            result &= ConsolidateMeterData(meterData, gasData, stroomData);
            return result;
        }

        private bool ConsolidateMeterData(
            TimeStampedDataList<MeterData> meterData, 
            TimeStampedDataList<GasData> gasData, 
            TimeStampedDataList<StroomData> stroomData
        ) {
            foreach (TimeStamp stamp in inputData.TimeStamps) {
                if (!gasData.TryGet(stamp, out GasData? gas)) {
                    Tracer.Trace("Missing gas data for " + stamp);
                    // sometimes on DST changes mismatches occur
                    // TODO: get rid of neeed for fallback, likely some subtlety in DST related parsing
                    TimeStamp fallback = stamp + TimeStamp.Duration;
                    if (!gasData.TryGet(fallback, out gas)) {
                        return false;
                    } else {
                        Tracer.Trace("Using fallback from " + fallback + " instead of " + stamp);
                    }
                }
                if (!stroomData.TryGet(stamp, out StroomData? stroom)) {
                    Tracer.Trace("Missing stroom data for " + stamp);
                    TimeStamp fallback = stamp + TimeStamp.Duration;
                    if (!stroomData.TryGet(stamp, out stroom)) {
                        return false;
                    } else {
                        Tracer.Trace("Using fallback from " + fallback);
                    }
                }
                try {
                    var meter = new MeterData() { TimeStamp = stamp, M3Gas = gas.M3, KwhVerbruik = stroom.KwhVerbruik, KwhTeruglevering = stroom.KwhTeruglevering, Temperatuur = gas.Temperatuur };
                    meterData.Add(meter);
                } catch (Exception e) {
                    Tracer.Trace("Failed to create dynamische prijs for " + stamp + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private bool ReadGasverbruik(
            ExcelReader reader, 
            TimeStampedDataList<GasData> gasData
        ) {
            reader.CheckHeader("datum_tijd", "levering", "buitentemperatuur");
            DateTime? previous = null;
            var delta = TimeSpan.FromMinutes(60);
            bool deltaChecked = false;
            try {
                double? previousTemperatuur = null;
                while (reader.Read(out string[] row)) {
                    string datumTijdString = row[0];
                    string m3String = row[1];
                    string temperatuurString = row[2];
                    if (!datumTijdString.TryParseDateTime(dateTimeFormats, out DateTime? dateTime)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumTijdString);
                    }
                    if (!deltaChecked && previous != null) {
                        if (delta != dateTime.Value - previous.Value) {
                            throw new InvalidDataException(
                                $"Invalid delta for {datumTijdString} in {reader.File.FullName}"
                            );
                        }
                        deltaChecked = true;
                    }
                    // dateTime - delta is a problem on daylight saving time switched, sometimes you need to jump an additional hour
                    // hence first conver to UTC then subtract, them convert back to local time
                    var stamp = new TimeStamp(dateTime.Value.ToUniversalTime() - delta);
                    if (!inputData.TimeStamps.Contains(stamp)) {
                        continue;
                    }
                    if (!m3String.TryParseDutch(out double m3)) {
                        throw reader.InvalidRow("Failed to parse m3 " + m3String);
                    }
                    double temperatuur;
                    if (temperatuurString == "-") {
                        #pragma warning disable CS8629 // Nullable value type may be null.
                        // from current data this does not (yet) happen
                        temperatuur = previousTemperatuur.Value;
                        #pragma warning restore CS8629 // Nullable value type may be null.
                    } else if (!temperatuurString.TryParseDutch(out temperatuur)) {
                        throw reader.InvalidRow("Failed to parse temperatuur " + temperatuurString);
                    }
                    var gas = new GasData { TimeStamp = stamp, M3 = m3, Temperatuur = temperatuur };
                    if (!gasData.TryGet(stamp, out GasData? existing)) {
                        existing = new GasData { TimeStamp = stamp };
                        gasData.Add(existing);
                    }
                    previousTemperatuur = temperatuur;
                    previous = dateTime;
                    existing.Add(gas);
                }
            } catch(Exception e) {
                Tracer.Trace($"Failed to read gasverbruik: {e.Message} from {reader.File.FullName}");
                return false;
            }
            return true;
        }

        private bool ReadStroomVerbruik(
            ExcelReader reader, 
            TimeStampedDataList<StroomData> stroomData
        ) {
            reader.CheckHeader("datum_tijd", "levering_normaal", "levering_laag", "teruglevering_normaal", "teruglevering_laag", "buitentemperatuur");
            DateTime? previous = null;
            var delta = TimeSpan.FromMinutes(15);
            bool deltaChecked = false;
            try {

                while (reader.Read(out string[] row)) {
                    string datumTijdString = row[0];
                    string leveringNormaalString = row[1];
                    string leveringLaagString = row[2];
                    string terugleveringNormaalString = row[3];
                    string terugleveringLaagString = row[4];
                    if (!datumTijdString.TryParseDateTime(dateTimeFormats, out DateTime? dateTime)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumTijdString);
                    }
                    if (!deltaChecked && previous != null) {
                        if (delta != dateTime.Value - previous.Value) {
                            throw new InvalidDataException(
                                $"Invalid delta for {datumTijdString} in {reader.File.FullName}"
                            );
                        }
                        deltaChecked = true;
                    }
                    // dateTime - delta is a problem on daylight saving time switched, sometimes you need to jump an additional hour
                    // hence first conver to UTC then subtract, them convert back to local time
                    var stamp = new TimeStamp(dateTime.Value.ToUniversalTime() - delta);
                    if (!inputData.TimeStamps.Contains(stamp)) {
                        continue;
                    }
                    if (!leveringNormaalString.TryParseDutchAllowEmpty(out double leveringNormaal)) {
                        throw reader.InvalidRow("Failed to parse levering normaal: " + leveringNormaalString);
                    }
                    if (!leveringLaagString.TryParseDutchAllowEmpty(out double leveringLaag)) {
                        throw reader.InvalidRow("Failed to parse levering laag: " + leveringLaagString);
                    }
                    if (!terugleveringNormaalString.TryParseDutchAllowEmpty(out double terugLeveringNormaal)) {
                        throw reader.InvalidRow("Failed to parse terug levering normaal: " + terugleveringNormaalString);
                    }
                    if (!terugleveringLaagString.TryParseDutchAllowEmpty(out double terugLeveringLaag)) {
                        throw reader.InvalidRow("Failed to parse terug levering laag: " + terugleveringLaagString);
                    }
                    var stroom = new StroomData { TimeStamp = stamp, KwhVerbruik = leveringNormaal + leveringLaag, KwhTeruglevering = terugLeveringLaag + terugLeveringNormaal };
                    if (!stroomData.TryGet(stamp, out StroomData? existing)) {
                        existing = new StroomData { TimeStamp = stamp };
                        stroomData.Add(existing);
                    }
                    previous = dateTime;
                    existing.Add(stroom);
                }
            } catch (Exception e) {
                Tracer.Trace($"Failed to read stroomverbruik: {e.Message} from {reader.File.FullName}");
                return false;
            }
            return true;
        }

        
    }
}
