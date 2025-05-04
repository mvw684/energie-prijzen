// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Prijzen {
    internal class StroomPrijs : AggregatableDataBase<StroomPrijs> {

        private double kwHprijs;

        /// <summary>
        /// Prijs per kWh in euro's.
        /// </summary>
        public double KwHPrijs {
            get => kwHprijs;
            init => kwHprijs = value;
        }

        public override StroomPrijs Aggregate(List<StroomPrijs> data) {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate.");
            }
            if (data.Count == 1) {
                return data[0];
            }
            double stroomPrijs = 0;
            foreach (var toAggregate in data) {
                if (TimeStamp != toAggregate.TimeStamp) {
                    throw new ArgumentException("All data must have the same timestamp.");
                }
                stroomPrijs += toAggregate.KwHPrijs;
            }
            return new StroomPrijs {
                TimeStamp = TimeStamp,
                KwHPrijs = stroomPrijs / data.Count
            };
        }

        public override void SetAggregateResult(StroomPrijs data) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.TimeStamp != TimeStamp) {
                throw new ArgumentException("Data must have the same timestamp.");
            }
            kwHprijs = data.KwHPrijs;
        }
    }
}
