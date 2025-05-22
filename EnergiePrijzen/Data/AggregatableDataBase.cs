// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    public abstract class AggregatableDataBase<TData> : IAggregatableData<TData> where TData : IAggregatableData<TData> {

        private readonly TimeStamp timestamp;
        private readonly List<TData> data = new List<TData>();

        public TimeStamp TimeStamp {
            get => timestamp;
            init => timestamp = value;
        }

        public void Add(TData item) {
            if (item.TimeStamp != timestamp) {
                throw new ArgumentException("All data must have the same timestamp.");
            }
            data.Add(item);
        }

        public virtual void Aggregate() {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate");
            }
            if (data.Count == 1) {
                SetAggregateResult(data[0]);
            } else {
                SetAggregateResult(Aggregate(data));
            }
        }

        public abstract TData Aggregate(List<TData> data);
            
        public abstract void SetAggregateResult(TData data);
    }
}
