// Copyright (c) 2025 mvw684

namespace EnergiePrijzen.Data {
    public class TimeStampedDataBase<TData> : ITimeStampedData<TData> {
        
        private TimeStamp timestamp;
        public required TimeStamp TimeStamp {
            get => timestamp;
            init => timestamp = value;
        }
    }
}
