// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class StroomData : AggregatableDataBase<StroomData> {

        private double kwhVerbruik;
        private double kwhTeruglevering;

        public double KwhVerbruik {
            get => kwhVerbruik; 
            init => kwhVerbruik = value;
        }

        public double KwhTeruglevering {
            get => kwhTeruglevering;
            init => kwhTeruglevering = value;
        }


        public override StroomData Aggregate(List<StroomData> data) {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate.");
            }
            if (data.Count == 1) {
                return data[0];
            }

            double verbuik = 0;
            double teruglevering = 0;
            foreach (StroomData toAggregate in data) {
                if (TimeStamp != toAggregate.TimeStamp) {
                    throw new ArgumentException("All data must have the same timestamp.");
                }
                teruglevering += toAggregate.KwhTeruglevering;
                kwhVerbruik += toAggregate.KwhVerbruik;
            }
            return new StroomData {
                TimeStamp = TimeStamp,
                KwhTeruglevering = teruglevering,
                KwhVerbruik = verbuik
            };
        }
        public override void SetAggregateResult(StroomData data) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.TimeStamp != TimeStamp) {
                throw new ArgumentException("Data must have the same timestamp.");
            }
            kwhVerbruik = data.KwhVerbruik;
            kwhTeruglevering = data.KwhTeruglevering;
        }
    }
}
