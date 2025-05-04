// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    public interface IAggregatableData<TData> : ITimeStampedData<TData> {

        public abstract void Aggregate();
    }
}
