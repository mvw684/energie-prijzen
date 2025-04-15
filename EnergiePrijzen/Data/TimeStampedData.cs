// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    internal class TimeStampedData<TData> where TData : IAggregatableData<TData>, new() {
        private readonly TimeStamp timestamp;
        private TData? aggregated;
        private readonly List<TData> raw;

        public TimeStampedData(TimeStamp timestamp) {
            this.timestamp = timestamp;
            raw = [];
        }

        public TimeStamp TimeStamp {
            get => timestamp;
        }

        public void Aggregate() {
            if(aggregated is null) {
                if(raw.Count == 0) {
                    aggregated = new TData();
                } else {
                    aggregated = TData.Aggregate(raw);
                }
            }
        }
    }
}
