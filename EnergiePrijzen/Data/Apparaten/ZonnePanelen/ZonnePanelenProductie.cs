// Copyright (c) 2025 mvw684

namespace EnergiePrijzen.Data.Apparaten.ZonnePanelen {
    internal class ZonnePanelenProductie : TimeStampedDataBase<ZonnePanelenProductie> {
        private double kwhProductie;

        public required double KwhProductie {
            get => kwhProductie;
            init => kwhProductie = value;
        }
    }
}
