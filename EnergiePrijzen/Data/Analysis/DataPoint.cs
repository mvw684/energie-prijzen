// Copyright (c) 2025 mvw684

using EnergiePrijzen.Data.Prijzen;

namespace EnergiePrijzen.Data.Analysis {
    internal class DataPoint  : TimeStampedDataBase<DataPoint> {
        
        internal required DynamischePrijs Prijs {
            get; init;
        }
    }
}
