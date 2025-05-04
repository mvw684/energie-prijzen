// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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

        private static readonly NumberStyles numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands;
        private static readonly CultureInfo dutch = new CultureInfo("nl-NL");

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
            result &= AggregateStromPrijzen();
            result &= AggregateGasPrijzen();
            Tracer.Trace("Loading jeroen prijzen " + (result ? "succeeded" : "failed"));
            return result;
        }

        private bool AggregateGasPrijzen() {
            foreach (var gasPrijs in gasPrijzen) {
                gasPrijs.Aggregate();
            }
            return true;
        }

        private bool AggregateStromPrijzen() {
            foreach (var stroomPrijs in stroomPrijzen) {
                stroomPrijs.Aggregate();
            }
            return true;
        }

        private bool LoadStroomPrijzen(DirectoryInfo folder) {
            foreach (var file in folder.EnumerateFiles("*stroomprijzen*.csv", SearchOption.TopDirectoryOnly)) {
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
                    var datumString = row[0];
                    var prijsString = row[1];

                    if (datumString.TryParseDateTime(dateTimeFormats, out TimeStamp? stamp)) {
                        if (!inputData.TimeStamps.Contains(stamp.Value)) {
                            continue;
                        }
                        if (double.TryParse(prijsString, numberStyles, dutch, out double prijs)) {
                            var stroomPrijs = new StroomPrijs { TimeStamp = stamp.Value, KwHPrijs = prijs };
                            if (!stroomPrijzen.TryGet(stamp.Value, out var existing)) {
                                existing = new StroomPrijs { TimeStamp = stamp.Value };
                                stroomPrijzen.Add(existing);
                            }
                            existing.Add(stroomPrijs);
                        } else {
                            reader.ThrowInvalidRow("Failed to parse prijs " + prijsString);
                        }
                    } else {
                        reader.ThrowInvalidRow("Failed to parse date time " + datumString);
                    }
                }
            }
        }

        private bool LoadGasPrijzen(DirectoryInfo folder) {
            foreach (var file in folder.EnumerateFiles("*gasprijzen*.csv", SearchOption.TopDirectoryOnly)) {
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
                    var datumString = row[0];
                    var prijsString = row[2];

                    if (datumString.TryParseDateTime(dateTimeFormats, out TimeStamp? stamp)) {
                        if (!inputData.TimeStamps.Contains(stamp.Value)) {
                            continue;
                        }
                        if (double.TryParse(prijsString, numberStyles, dutch, out double prijs)) {
                            var gasPrijs = new GasPrijs { TimeStamp = stamp.Value, M3Prijs = prijs };
                            if (!gasPrijzen.TryGet(stamp.Value, out var existing)) {
                                existing = new GasPrijs { TimeStamp = stamp.Value };
                                gasPrijzen.Add(existing);
                            }
                            existing.Add(gasPrijs);
                        } else {
                            reader.ThrowInvalidRow("Failed to parse prijs " + prijsString);
                        }
                    } else {
                        reader.ThrowInvalidRow("Failed to parse date time " + datumString);
                    }
                }
            }
        }


    }
}
