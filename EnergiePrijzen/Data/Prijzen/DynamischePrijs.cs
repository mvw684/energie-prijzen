// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Prijzen {
    public class DynamischePrijs : AggregatedDataBase<DynamischePrijs> {
        private double kwHprijs;
        private double m3Prijs;
        
        /// <summary>
        /// Prijs per kWh in euro's.
        /// </summary>
        public double KwHPrijs {
            get => kwHprijs;
            init => kwHprijs = value;
        }

        public double M3Prijs {
            get => m3Prijs;
            init => m3Prijs = value;
        }

        public static DynamischePrijs Aggregate(List<DynamischePrijs> data) {
            throw new ArgumentException("Dynamic data is not aggregatable");
        }
    }
}
