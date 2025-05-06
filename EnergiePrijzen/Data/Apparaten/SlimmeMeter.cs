// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Apparaten {
    internal class SlimmeMeter {
        private const string dateTimeFormat = "dd-MM-yyyy HH:mm:ss K";
        private static string[] dateTimeFormats = [dateTimeFormat];

        private InputData inputData;
        public SlimmeMeter(InputData inputData) => this.inputData = inputData;

        public bool Load(out TimeStampedDataList<MeterData> meterData) {
            meterData = new TimeStampedDataList<MeterData>();
            var meterFolder = new DirectoryInfo(inputData.Settings.SlimmeMeter);
            bool result = ReadMeterData(meterFolder, meterData);
            return result;
        }

        private bool ReadMeterData(
            DirectoryInfo meterFolder, 
            TimeStampedDataList<MeterData> meterData
        ) {
            bool result = true;
            var gasData = new TimeStampedDataList<GasData>();
            var stroomData = new TimeStampedDataList<StroomData>();
            foreach (var fileInfo in meterFolder.EnumerateFiles("*.xls", SearchOption.TopDirectoryOnly)) {
                try {
                    using (var reader = new ExcelReader(fileInfo)) {
                        var header = reader.Header;
                        GC.KeepAlive(header);
                        if (header.Length == 3) {
                            result &= ReadGasverbruik(reader, gasData);
                        } else {
                            result &= ReadStroomVerbruik(reader, stroomData);
                        }
                    }
                } catch (Exception ex) {
                    Tracer.Trace($"Error reading file {fileInfo.FullName}: {ex.Message}");
                    result = false;
                }
            }
            result &= gasData.Aggregate();
            result &= stroomData.Aggregate();
            result &= Consolidate(meterData, gasData, stroomData);
            return result;
        }

        private bool Consolidate(
            TimeStampedDataList<MeterData> meterData, 
            TimeStampedDataList<GasData> gasData, 
            TimeStampedDataList<StroomData> stroomData
        ) => throw new NotImplementedException();

        private bool ReadGasverbruik(
            ExcelReader reader, 
            TimeStampedDataList<GasData> gasData
        ) {
            reader.CheckHeader("datum_tijd", "levering", "buitentemperatuur");
            DateTime? previous = null;
            TimeSpan delta = TimeSpan.FromMinutes(60);
            bool deltaChecked = false;
            try {

                while (reader.Read(out string[] row)) {
                    var datumTijdString = row[0];
                    var m3String = row[1];
                    var temperatuurString = row[2];
                    if (!datumTijdString.TryParseDateTime(dateTimeFormats, out DateTime? dateTime)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumTijdString);
                    }
                    if (previous is null) {
                        previous = dateTime;
                    } else if (!deltaChecked) {
                        if (delta != dateTime.Value - previous.Value) {
                            throw new InvalidDataException(
                                $"Invalid delta for {datumTijdString} in {reader.File.FullName}"
                            );
                        }
                        deltaChecked = true;
                    }

                    var stamp = new TimeStamp(dateTime.Value - delta);
                    if (!inputData.TimeStamps.Contains(stamp)) {
                        continue;
                    }
                    if (!m3String.TryParseDutch(out double m3)) {
                        throw reader.InvalidRow("Failed to parse m3 " + m3String);
                    }
                    if (!temperatuurString.TryParseDutch(out double temperatuur)) {
                        throw reader.InvalidRow("Failed to parse temperatuur " + temperatuurString);
                    }
                    var gas = new GasData { TimeStamp = stamp, M3 = m3, Temperatuur = temperatuur };
                    if (!gasData.TryGet(stamp, out var existing)) {
                        existing = new GasData { TimeStamp = stamp };
                        gasData.Add(existing);
                    }
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
            TimeSpan delta = TimeSpan.FromMinutes(15);
            bool deltaChecked = false;
            try {

                while (reader.Read(out string[] row)) {
                    var datumTijdString = row[0];
                    var leveringNormaalString = row[1];
                    var leveringLaagString = row[2];
                    var terugleveringNormaalString = row[3];
                    var terugleveringLaagString = row[4];
                    if (!datumTijdString.TryParseDateTime(dateTimeFormats, out DateTime? dateTime)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumTijdString);
                    }
                    if (previous is null) {
                        previous = dateTime;
                    } else if (!deltaChecked) {
                        if (delta != dateTime.Value - previous.Value) {
                            throw new InvalidDataException(
                                $"Invalid delta for {datumTijdString} in {reader.File.FullName}"
                            );
                        }
                        deltaChecked = true;
                    }

                    var stamp = new TimeStamp(dateTime.Value - delta);
                    if (!inputData.TimeStamps.Contains(stamp)) {
                        continue;
                    }
                    if (!leveringNormaalString.TryParseDutch(out double leveringNormaal)) {
                        throw reader.InvalidRow("Failed to parse levering normaal: " + leveringNormaalString);
                    }
                    if (!leveringLaagString.TryParseDutch(out double leveringLaag)) {
                        throw reader.InvalidRow("Failed to parse levering laag: " + leveringLaagString);
                    }
                    if (!terugleveringNormaalString.TryParseDutch(out double terugLeveringNormaal)) {
                        throw reader.InvalidRow("Failed to parse terug levering normaal: " + terugleveringNormaalString);
                    }
                    if (!terugleveringLaagString.TryParseDutch(out double terugLeveringLaag)) {
                        throw reader.InvalidRow("Failed to parse terug levering laag: " + terugleveringLaagString);
                    }

                    var stroom = new StroomData { TimeStamp = stamp, KwhVerbruik = leveringNormaal + leveringLaag, KwhTeruglevering = terugLeveringLaag + terugLeveringNormaal };
                    if (!stroomData.TryGet(stamp, out var existing)) {
                        existing = new StroomData { TimeStamp = stamp };
                        stroomData.Add(existing);
                    }
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
