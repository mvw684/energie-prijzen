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


        internal const string DateTimeFormat = "yyyy-MM-dd HH:mm";
        private readonly static string[] dateTimeFormats = [TimeStampExtensions.DateFormat];

        private static readonly NumberStyles numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands;
        private static readonly CultureInfo dutch = new CultureInfo("nl-NL");

        public JeroenPrijzen(InputData inputData) => this.inputData = inputData;

        internal bool Load([NotNullWhen(true)] out TimeStampedDataList<DynamischePrijs> dynamischePrijzen) {
            dynamischePrijzen = new TimeStampedDataList<DynamischePrijs>();
            stroomPrijzen = new TimeStampedDataList<StroomPrijs>();
            gasPrijzen = new TimeStampedDataList<GasPrijs>();


        var folder = new DirectoryInfo(inputData.Settings.JeroenPrijzen);
            bool result = true;
            foreach(var file in folder.EnumerateFiles("*stroomprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                result &= ReadStroomPrijzen(file);
            }

            foreach(var file in folder.EnumerateFiles("*gasprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                result &= ReadGasPrijzen(file);
            }
            return result;
        }

        private bool ReadStroomPrijzen(FileInfo file) {

            using (var reader = new CsvReader(file)) {
                if (!reader.Hasheader) {
                    return false;
                }
                var header = reader.Header;
                if (header.Length != 2) {
                    return false;
                }
                if (header[0] != "datum" || header[1] != "prijs_excl_btw") {
                    return false;
                }
                while (reader.Read(out string[] row)) {
                    var datumString = row[0];
                    var prijsString = row[1];

                    if (datumString.TryParseDateTime(dateTimeFormats, out TimeStamp? stamp)) {
                        if (double.TryParse(prijsString, numberStyles, dutch, out double prijs)) {
                            var stroomPrijs = new StroomPrijs { TimeStamp = stamp.Value, KwHPrijs = prijs };
                            if (!stroomPrijzen.TryGet(stamp.Value, out var existing)) {
                                existing = new StroomPrijs { TimeStamp = stamp.Value };
                                stroomPrijzen.Add(existing);
                            }
                            existing.Add(stroomPrijs);
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ReadGasPrijzen(FileInfo file) {
            GC.KeepAlive(file);
            gasPrijzen.Add(new GasPrijs { TimeStamp = new TimeStamp(), M3Prijs = 0 });
            return false;
        }


    }
}
