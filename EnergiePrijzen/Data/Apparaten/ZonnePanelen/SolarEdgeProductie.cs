// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.ZonnePanelen {
    internal class SolarEdgeProductie : AggregatableDataBase<SolarEdgeProductie> {
        private double whProductie;

        public double WhProductie {
            get => whProductie;
            init => whProductie = value;
        }

        public override SolarEdgeProductie Aggregate(List<SolarEdgeProductie> data) {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate.");
            }
            if (data.Count == 1) {
                return data[0];
            }
            double whProductie = 0;
            foreach (var toAggregate in data) {
                if (TimeStamp != toAggregate.TimeStamp) {
                    throw new ArgumentException("All data must have the same timestamp.");
                }
                whProductie += toAggregate.WhProductie;
            }
            return new SolarEdgeProductie {
                TimeStamp = TimeStamp,
                WhProductie = whProductie
            };
        }

        public override void SetAggregateResult(SolarEdgeProductie data) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.TimeStamp != TimeStamp) {
                throw new ArgumentException("Data must have the same timestamp.");
            }
            whProductie = data.WhProductie;
        }
    }
}
