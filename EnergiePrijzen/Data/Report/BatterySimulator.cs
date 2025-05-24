// Copyright (c) 2025 mvw684

using System;

namespace EnergiePrijzen.Data.Report {
    internal class BatterySimulator {
        const double maxKwhCharche = 2 *2200;
        const double maxKwhDischarge = 2 * 2200;
        const double maxKwhStored = 10000;
        double kwhStored = 0;

        internal void Simulate(ReportRowData reportRow) => throw new NotImplementedException();
    }
}
