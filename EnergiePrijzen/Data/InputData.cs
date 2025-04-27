// Copyright (c) 2025 mvw684

using EnergiePrijzen.Config;

namespace EnergiePrijzen.Data {
    internal class InputData {
        internal required TimeStamp Start {
            get; init;
        }
        internal required TimeStamp End {
            get; init;
        }
        internal required Settings Settings {
            get; init;
        }

        internal required UniqueItemList<TimeStamp> TimeStamps {
            get; init;
        }
    }
}
