// Copyright (c) 2025 mvw684

using System.Collections.Generic;

namespace EnergiePrijzen.Data.Apparaten.Meter {
    internal class GasData : AggregatableDataBase<GasData> {

        private double m3gas;
        private double temperatuur;

        public double M3Gas {
            get => m3gas;
            init => m3gas = value;
        }

        public double Temperatuur {
            get => temperatuur;
            init => temperatuur = value;
        }


        public override GasData Aggregate(List<GasData> data) => throw new System.NotImplementedException();
        public override void SetAggregateResult(GasData data) => throw new System.NotImplementedException();
    }
}
