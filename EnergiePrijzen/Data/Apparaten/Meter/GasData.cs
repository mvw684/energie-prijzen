// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class GasData : AggregatableDataBase<GasData> {

        private double m3;
        private double temperatuur;

        public double M3 {
            get => m3;
            init => m3 = value;
        }

        public double Temperatuur {
            get => temperatuur;
            init => temperatuur = value;
        }


        public override GasData Aggregate(List<GasData> data) {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate.");
            }
            if (data.Count == 1) {
                return data[0];
            }

            double temperatuur = 0;
            double m3 = 0;
            foreach (var toAggregate in data) {
                if (TimeStamp != toAggregate.TimeStamp) {
                    throw new ArgumentException("All data must have the same timestamp.");
                }
                m3 += toAggregate.M3;
                temperatuur += toAggregate.Temperatuur;
            }
            temperatuur /= data.Count;
            return new GasData {
                TimeStamp = TimeStamp,
                Temperatuur = temperatuur,
                M3 = m3
            };
        }

        public override void SetAggregateResult(GasData data) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.TimeStamp != TimeStamp) {
                throw new ArgumentException("Data must have the same timestamp.");
            }
            m3 = data.M3;
            temperatuur = data.Temperatuur;
        }
    }
}
