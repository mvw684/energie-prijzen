// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class GasData : AggregatableDataBase<GasData> {

        private double m3;
        private double temperatuur;

        public double M3 {
            get => m3;
            init => m3 = value;
        }

        public double Temperatuur {
            get => temperatuur;
            init => temperatuur = value;
        }


        public override GasData Aggregate(List<GasData> data) => throw new System.NotImplementedException();
        public override void SetAggregateResult(GasData data) => throw new System.NotImplementedException();
    }
}
