// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    internal class TimeStampedData<TData> where TData : IAggregatableData<TData>, new() {
        private DateTime hour;
        private TData? aggregated;
        private List<TData> raw;

        public TimeStampedData(DateTime hour) {
            this.hour = hour;
            this.raw = new List<TData>();
        }

        public DateTime Hour {
            get => hour;
        }

        void Aggregate() {
            if(aggregated is null) {
                if(raw.Count == 0) {
                    aggregated = new TData();
                } else {
                    aggregated = new TData().Aggregate(raw);
                }
            }
        }
    }
}
