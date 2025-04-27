// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    public interface IAggregatableData<TData> : ITimeStampedData<TData> {

        public abstract TData AggregateData();

        public static abstract TData Aggregate(List<TData> data);
    }
}
