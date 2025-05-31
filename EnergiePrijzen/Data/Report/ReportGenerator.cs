// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Csv;
using EnergiePrijzen.Data.Prijzen;

namespace EnergiePrijzen.Data.Report {
    internal class ReportGenerator {
        private readonly TimeStampedDataList<DynamischePrijs> prijzen;
        private readonly TimeStampedDataList<MeterData> meterData;
        private readonly InputData inputData;
        private FileInfo file;
        internal  required TimeStampedDataList<DynamischePrijs> Prijzen {
            get; init;
        }

        internal required TimeStampedDataList<MeterData> MeterData {
            get; init;
        }

        internal required InputData InputData {
            get; init;
        }

        internal bool Generate() {
            try {
                file = new FileInfo(Path.Combine(InputData.Settings.Resultaat, "EnergiePrijzenReport-" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss")+ ".xlsx"));
                Tracer.Trace("Generating report for " + InputData.Start + " to " + InputData.End + " in " + file.FullName);
                if (
                    !MergeDataValues(
                        out TimeStampedDataList<ReportRowData>? reportData, 
                        out string? message
                    )
                ) {
                    throw new InvalidDataException(message);
                }
                SaveDataValues(reportData);
                Tracer.Trace("Report completed");
                _ = Process.Start(file.FullName);
            } catch(Exception e) {
                Tracer.Trace("Failed to generate report: " + e.Message);
            }
            return false;
        }

        private bool MergeDataValues([NotNullWhen(true)] out TimeStampedDataList<ReportRowData> reportData, [NotNullWhen(false)] out string message) {
            message = string.Empty;
            reportData = new TimeStampedDataList<ReportRowData>();
            var batterySimulator = new BatterySimulator();
            var prijsBerekening = new PrijsBerekening();
            foreach (TimeStamp timeStamp in inputData.TimeStamps) {
                if (!prijzen.TryGet(timeStamp, out DynamischePrijs? prijs)) {
                    message = "Missing price data for " + timeStamp;
                    return false;
                }
                if (!meterData.TryGet(timeStamp, out MeterData? meter)) {
                    message = "Missing meter data for " + timeStamp;
                    return false;
                }
                var reportRow = new ReportRowData() {
                    TimeStamp = timeStamp,
                    M3Prijs = prijs.M3Prijs,
                    KwhPrijs = prijs.KwHPrijs,
                    M3Gas = meter.M3Gas,
                    KwhVerbruik = meter.KwhVerbruik,
                    KwhTeruglevering = meter.KwhTeruglevering,
                    Temperatuur = meter.Temperatuur,
                    KwhBatterijLaden = 0,
                    KwhBatterijOntladen = 0,
                    KwhBatterijStored = 0,
                    BatterijPercentageFull = 0
                };
                batterySimulator.Simulate(reportRow);
                prijsBerekening.Compute(reportRow);
                reportData.Add(reportRow);
            }
            return true;
        }

        private void SaveDataValues(TimeStampedDataList<ReportRowData> reportData) {
            using (var writer = new ExcelWriter(file, "Prijzen en verbruik")) {
                ReportRowData.WriteHeader(writer);
                foreach (ReportRowData row in reportData) {
                    row.WriteRow(writer);
                }
                ReportRowData.Format(writer);
            }
        }
    }
}
