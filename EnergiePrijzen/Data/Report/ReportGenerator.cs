// Copyright (c) 2025 mvw684

using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Prijzen;

namespace EnergiePrijzen.Data.Report {
    internal class ReportGenerator {
        private readonly TimeStampedDataList<DynamischePrijs> prijzen;
        private readonly TimeStampedDataList<MeterData> meterData;
        private readonly InputData inputData;

        internal  required TimeStampedDataList<DynamischePrijs> Prijzen {
            get; init;
        }

        internal required TimeStampedDataList<MeterData> MeterData {
            get; init;
        }

        internal required InputData InputData {
            get; init;
        }


    }
}
