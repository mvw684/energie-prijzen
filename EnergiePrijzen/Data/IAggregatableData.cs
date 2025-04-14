// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    internal interface IAggregatableData<TData> {
        TData Aggregate(List<TData> data);
    }
}
