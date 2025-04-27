
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

        public static TData Aggregate(List<TData> data) => throw new NotImplementedException();

        public void Add(TData item) {
            if (item.TimeStamp != timestamp) {
                throw new ArgumentException("All data must have the same timestamp.");
            }
            data.Add(item);
        }

        public virtual TData AggregateData() {
            if (data.Count == 0) {
                throw new ArgumentException("No data to aggregate");
            }
            return TData.Aggregate(data);
        }
    }
}
