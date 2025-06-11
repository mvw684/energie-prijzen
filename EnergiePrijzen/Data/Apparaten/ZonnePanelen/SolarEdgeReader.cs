// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Apparaten.ZonnePanelen {
    internal class SolarEdgeReader {
        private readonly InputData inputData;
        private readonly TimeStampedDataList<SolarEdgeProductie> solarEdgeProductie = new TimeStampedDataList<SolarEdgeProductie>();
        
        private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private readonly static string[] dateTimeFormats = [dateTimeFormat];

        public SolarEdgeReader(InputData inputData) => this.inputData = inputData;

        internal bool Load([NotNullWhen(true)] out TimeStampedDataList<ZonnePanelenProductie> zonnepanelenProductie) {
            bool result = true;
            Tracer.Trace("Loading SolarEdge data");
            var folder = new DirectoryInfo(inputData.Settings.SolarEdge);
            result &= LoadSolarEdgeData(folder);
            result &= solarEdgeProductie.Aggregate();
            result &= BuildResult(out zonnepanelenProductie);
            Tracer.Trace("Loading SolarEdge data " + (result ? "succeeded" : "failed"));
            return result;
        }

        private bool BuildResult([NotNullWhen(true)] out TimeStampedDataList<ZonnePanelenProductie> zonnepanelenProductie) {

            zonnepanelenProductie = new TimeStampedDataList<ZonnePanelenProductie>();

            foreach (TimeStamp stamp in inputData.TimeStamps) {
                if (!solarEdgeProductie.TryGet(stamp, out SolarEdgeProductie? solarEdgeProductieForTimeStamp)) {
                    Tracer.Trace("Missing Solaredge data for " + stamp);
                    return false;
                }
                try {
                    var panelenProductie = new ZonnePanelenProductie() { TimeStamp = solarEdgeProductieForTimeStamp.TimeStamp, KwhProductie = solarEdgeProductieForTimeStamp.WhProductie / 1000d };
                    zonnepanelenProductie.Add(panelenProductie);
                } catch (Exception e) {
                    Tracer.Trace("Failed to create ZonnePaneel productie for " + stamp + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private bool LoadSolarEdgeData(DirectoryInfo folder) {
            foreach (FileInfo file in folder.EnumerateFiles("*365Zon-5691KB472026590-15min.csv", SearchOption.TopDirectoryOnly)) {
                try {
                    LoadSolarEdgeData(file);
                } catch (Exception e) {
                    Tracer.Trace("Failed to read stroomprijzen from " + file.FullName + ": " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private void LoadSolarEdgeData(FileInfo file) {
            Tracer.Trace("Reading " + file.FullName);
            // dateTime is related to the past 15 minutes, we need the start of the 'period'
            // analysing results indicates the date/time value might be from the start of the period
            var offset = TimeSpan.FromMinutes(0);
            using (var reader = new CsvReader(file, ";")) {

                reader.CheckHeader("date", "value");

                while (reader.Read(out string[] row)) {
                    string datumString = row[0];
                    string whString = row[1];

                    if (!datumString.TryParseDateTime(dateTimeFormats, out DateTime? dateTime)) {
                        throw reader.InvalidRow("Failed to parse date time " + datumString);
                    }
                    dateTime -= offset; // adjust to the start of the 15 minute period
                    var stamp = new TimeStamp(dateTime.Value);
                    if (!inputData.TimeStamps.Contains(stamp)) {
                        continue;
                    }
                    if (!whString.TryParseInvariant(out double whProductie)) {
                        throw reader.InvalidRow("Failed to parse kwh " + whString);
                    }
                    var productie = new SolarEdgeProductie { TimeStamp = stamp, WhProductie = whProductie };
                    if (!solarEdgeProductie.TryGet(stamp, out SolarEdgeProductie? existing)) {
                        existing = new SolarEdgeProductie { TimeStamp = stamp };
                        solarEdgeProductie.Add(existing);
                    }
                    existing.Add(productie);
                }
            }
        }
    }
}
