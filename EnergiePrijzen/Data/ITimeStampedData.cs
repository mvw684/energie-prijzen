// Copyright (c) 2025 mvw684

namespace EnergiePrijzen.Data {
    public interface ITimeStampedData<TData> {
        TimeStamp TimeStamp {
            get;
            init;
        }

    }
}
