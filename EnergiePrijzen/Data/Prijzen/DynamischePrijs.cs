// Copyright (c) 2025 mvw684

namespace EnergiePrijzen.Data.Prijzen {
    public class DynamischePrijs : TimeStampedDataBase<DynamischePrijs> {
        private double kwHprijs;
        private double m3Prijs;
        
        /// <summary>
        /// Prijs per kWh in euro's.
        /// </summary>
        public required double KwHPrijs {
            get => kwHprijs;
            init => kwHprijs = value;
        }

        public required double M3Prijs {
            get => m3Prijs;
            init => m3Prijs = value;
        }
    }
}
