// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Prijzen {
    internal class JeroenPrijzen {
        private readonly InputData inputData;
        private TimeStampedDataList<StroomPrijs> stroomPrijzen = new TimeStampedDataList<StroomPrijs>();
        private TimeStampedDataList<GasPrijs> gasPrijzen = new TimeStampedDataList<GasPrijs>();


        private const string dateTimeFormat1 = "yyyy-MM-dd HH:mm:ss";
        private const string dateTimeFormat2 = "yyyy-MM-dd HH:mm";
        private readonly static string[] dateTimeFormats = [dateTimeFormat1, dateTimeFormat2];

        public JeroenPrijzen(InputData inputData) => this.inputData = inputData;

        internal bool Load([NotNullWhen(true)] out TimeStampedDataList<DynamischePrijs> dynamischePrijzen) {
            bool result = true;
            Tracer.Trace("Loading Jeroen prijzen");
            dynamischePrijzen = new TimeStampedDataList<DynamischePrijs>();
            stroomPrijzen = new TimeStampedDataList<StroomPrijs>();
            gasPrijzen = new TimeStampedDataList<GasPrijs>();

            var folder = new DirectoryInfo(inputData.Settings.JeroenPrijzen);
            result &= LoadStroomPrijzen(folder);
            result &= LoadGasPrijzen(folder);
            result &= stroomPrijzen.Aggregate();
            result &= gasPrijzen.Aggregate();
            result &= ConsolidatePrijzen(dynamischePrijzen);
            Tracer.Trace("Loading jeroen prijzen " + (result ? "succeeded" : "failed"));
            return result;
        }

        private bool ConsolidatePrijzen(TimeStampedDataList<DynamischePrijs> dynamischePrijzen) {

            foreach (TimeStamp stamp in inputData.TimeStamps) {
                if (!gasPrijzen.TryGet(stamp, out GasPrijs? gasPrijs)) {
                    Tracer.Trace("Missing gas prijs for " + stamp);
                    return false;
                }
                if (!stroomPrijzen.TryGet(stamp, out StroomPrijs? stroomPrijs)) {
                    Tracer.Trace("Missing stroom prijs for " + stamp);
                    return false;
                }
                try {
                    var dynamischePrijs = new DynamischePrijs() { TimeStamp = stamp, KwHPrijs = stroomPrijs.KwHPrijs, M3Prijs = gasPrijs.M3Prijs };
                    dynamischePrijzen.Add(dynamischePrijs);
                } catch (Exception e) {
                    Tracer.Trace("Failed to create dynamische prijs for " + stamp + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private bool LoadStroomPrijzen(DirectoryInfo folder) {
            foreach (FileInfo file in folder.EnumerateFiles("*stroomprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                try {
                    ReadStroomPrijzen(file);
                } catch (Exception e) {
                    Tracer.Trace("Failed to read stroomprijzen from " + file.FullName + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private void ReadStroomPrijzen(FileInfo file) {
            Tracer.Trace("Reading " + file.FullName);
            using (var reader = new CsvReader(file, ",")) {

                reader.CheckHeader("datum", "prijs_excl_btw");
                
                while (reader.Read(out string[] row)) {
                    string datumString = row[0];
                    string prijsString = row[1];

                    if (!datumString.TryParseDateTime(dateTimeFormats, out TimeStamp? stamp)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumString);
                    }
                    if (!inputData.TimeStamps.Contains(stamp.Value)) {
                        continue;
                    }
                    if (!prijsString.TryParseDutch(out double prijs)) {
                        throw reader.InvalidRow("Failed to parse prijs " + prijsString);
                    }
                    var stroomPrijs = new StroomPrijs { TimeStamp = stamp.Value, KwHPrijs = prijs };
                    if (!stroomPrijzen.TryGet(stamp.Value, out StroomPrijs? existing)) {
                        existing = new StroomPrijs { TimeStamp = stamp.Value };
                        stroomPrijzen.Add(existing);
                    }
                    existing.Add(stroomPrijs);
                }
            }
        }

        private bool LoadGasPrijzen(DirectoryInfo folder) {
            foreach (FileInfo file in folder.EnumerateFiles("*gasprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                try {
                    ReadGasPrijzen(file);
                } catch (Exception e) {
                    Tracer.Trace("Failed to read gasprijzen from " + file.FullName + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private void ReadGasPrijzen(FileInfo file) {
            Tracer.Trace("Reading " + file.FullName);
            using (var reader = new CsvReader(file, ";")) {
                reader.CheckHeader("datum", "prijs_excl_belastingen");
                while (reader.Read(out string[] row)) {
                    string datumString = row[0];
                    string prijsString = row[2];

                    if (!datumString.TryParseDateTime(dateTimeFormats, out TimeStamp? stamp)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumString);
                    }
                    if (!inputData.TimeStamps.Contains(stamp.Value)) {
                        continue;
                    }
                    if (!prijsString.TryParseDutch(out double prijs)) {
                        throw reader.InvalidRow("Failed to parse prijs " + prijsString);
                    }
                    // gasprijzen zijn per dag. so need to add each hour/timestamp duration
                    TimeStamp? end = stamp + TimeSpan.FromHours(24);
                    // FIX: dst skipping/ignoring here???
                    while(stamp < end) {
                        var gasPrijs = new GasPrijs { TimeStamp = stamp.Value, M3Prijs = prijs };
                        if (!gasPrijzen.TryGet(stamp.Value, out GasPrijs? existing)) {
                            existing = new GasPrijs { TimeStamp = stamp.Value };
                            gasPrijzen.Add(existing);
                        }
                        existing.Add(gasPrijs);
                        stamp += TimeStamp.Duration;
                    }
                }
            }
        }
    }
}
