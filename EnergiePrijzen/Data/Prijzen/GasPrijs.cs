// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Prijzen {
    public class GasPrijs : AggregatableDataBase<GasPrijs> {
        
        private double m3Prijs;
        
        /// <summary>
        /// Prijs per M3 in euro's.
        /// </summary>
        public double M3Prijs {
            get => m3Prijs;
            init => m3Prijs = value;
        }

        public override GasPrijs Aggregate(List<GasPrijs> data) {
            if(data.Count == 0) {
                throw new ArgumentException("No data to aggregate.");
            }
            if (data.Count == 1) {
                return data[0];
            }
            double stroomPrijs = 0;
            foreach(var toAggregate in data) {
                if(TimeStamp != toAggregate.TimeStamp) {
                    throw new ArgumentException("All data must have the same timestamp.");
                }
                stroomPrijs += toAggregate.M3Prijs;
            }
            return new GasPrijs {
                TimeStamp = TimeStamp,
                M3Prijs = stroomPrijs / data.Count
            };
        }

        public override void SetAggregateResult(GasPrijs data) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.TimeStamp != TimeStamp) {
                throw new ArgumentException("Data must have the same timestamp.");
            }
            m3Prijs = data.M3Prijs;
        }
    }
}
