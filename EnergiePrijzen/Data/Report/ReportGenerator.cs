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

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        // required init only fields
        private readonly TimeStampedDataList<DynamischePrijs> prijzen;
        private readonly TimeStampedDataList<MeterData> meterData;
        private readonly InputData inputData;
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        internal required TimeStampedDataList<DynamischePrijs> Prijzen {
            get => prijzen; 
            init => prijzen = value ?? throw new ArgumentNullException(nameof(value), "Prijzen cannot be null. Ensure that the data is loaded correctly before generating the report.");
        }

        internal required TimeStampedDataList<MeterData> MeterData {
            get => meterData; 
            init => meterData = value ?? throw new ArgumentNullException(nameof(value), "MeterData cannot be null. Ensure that the data is loaded correctly before generating the report.");
        }

        internal required InputData InputData {
            get => inputData; 
            init => inputData = value ?? throw new ArgumentNullException(nameof(value), "InputData cannot be null. Ensure that the data is loaded correctly before generating the report.");
        }

        internal bool Generate() {
            try {
                var file = new FileInfo(Path.Combine(InputData.Settings.Resultaat, "EnergiePrijzenReport-" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".xlsx"));
                Tracer.Trace("Generating report for " + InputData.Start + " to " + InputData.End + " in " + file.FullName);
                if (
                    !MergeDataValues(
                        out TimeStampedDataList<ReportRowData>? reportData, 
                        out string? message
                    )
                ) {
                    throw new InvalidDataException(message);
                }
                SaveDataValues(file, reportData);
                Tracer.Trace("Report completed");
                var info = new ProcessStartInfo { FileName = file.FullName, UseShellExecute = true };
                _ = Process.Start(info);
            } catch(Exception e) {
                Tracer.Trace("Failed to generate report: " + e.Message);
                return false;
            }
            return true;
        }

        private bool MergeDataValues([NotNullWhen(true)] out TimeStampedDataList<ReportRowData> reportData, [NotNullWhen(false)] out string message) {
            message = string.Empty;
            reportData = new TimeStampedDataList<ReportRowData>();
            BatterySimulator? batterySimulator = inputData.Settings.NrOfBatteriesToSimulate > 0 ? new BatterySimulator(inputData.Settings.NrOfBatteriesToSimulate) : null;
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
                batterySimulator?.Simulate(reportRow);
                prijsBerekening.Compute(reportRow);
                reportData.Add(reportRow);
            }
            return true;
        }

        private static void SaveDataValues(FileInfo file, TimeStampedDataList<ReportRowData> reportData) {
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
