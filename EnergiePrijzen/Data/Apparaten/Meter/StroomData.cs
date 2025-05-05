// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class StroomData : AggregatableDataBase<StroomData> {

        private double kwhVerbruik;
        private double kwhTeruglevering;

        public double KwhVerbruik {
            get => kwhVerbruik; 
            init => kwhVerbruik = value;
        }

        public double KwhTeruglevering {
            get => kwhTeruglevering;
            init => kwhTeruglevering = value;
        }


        public override StroomData Aggregate(List<StroomData> data) => throw new System.NotImplementedException();
        public override void SetAggregateResult(StroomData data) => throw new System.NotImplementedException();
    }
}
