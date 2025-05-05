// Copyright (c) 2025 mvw684


namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class MeterData : TimeStampedDataBase<MeterData> {
        private double m3gas;
        private double kwhVerbruik;
        private double kwhTeruglevering;

        public required double KwhVerbruik {
            get => kwhVerbruik;
            init => kwhVerbruik = value;
        }

        public required double KwhTeruglevering {
            get => kwhTeruglevering;
            init => kwhTeruglevering = value;
        }

        public required double M3Gas {
            init => m3gas = value;
            get => m3gas;
        }
    }
}
