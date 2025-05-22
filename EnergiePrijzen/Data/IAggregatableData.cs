// Copyright (c) 2025 mvw684

namespace EnergiePrijzen.Data {
    public interface IAggregatableData<TData> : ITimeStampedData<TData> {

        public abstract void Aggregate();
    }
}
