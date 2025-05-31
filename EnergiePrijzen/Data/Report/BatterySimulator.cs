// Copyright (c) 2025 mvw684

using System;

namespace EnergiePrijzen.Data.Report {
    internal class BatterySimulator {
        const int nrOfBatteries = 2;
        const double maxKwhCharche = nrOfBatteries * 2000;
        const double maxKwhDischarge = nrOfBatteries * 1700;
        const double maxKwhStored = nrOfBatteries * 5000;
        double kwhStored = 0;

        internal void Simulate(ReportRowData reportRow) {
            if (reportRow.KwhTeruglevering > 0) {
                // Battery charging
                double kwhToCharge = Min(reportRow.KwhTeruglevering, maxKwhCharche);
                if (kwhStored + kwhToCharge > maxKwhStored) {
                    kwhToCharge = maxKwhStored - kwhStored; // Limit to max storage capacity
                }
                kwhStored += kwhToCharge;
                reportRow.KwhBatterijLaden = kwhToCharge;
                reportRow.KwhTeruglevering -= kwhToCharge; // Reduce the amount of energy returned to the grid
                reportRow.KwhBatterijStored = kwhStored;
                reportRow.BatterijPercentageFull = (kwhStored / maxKwhStored) * 100; // Update battery percentage
            } else if (reportRow.KwhVerbruik > 0) {
                // Battery discharging
                double kwhToDischarge = Min(reportRow.KwhVerbruik, maxKwhDischarge, kwhStored);
                kwhStored -= kwhToDischarge;
                reportRow.KwhBatterijOntladen = kwhToDischarge;
                reportRow.KwhVerbruik -= kwhToDischarge; // Reduce the amount of energy consumed from the grid
                reportRow.KwhBatterijStored = kwhStored;
                reportRow.BatterijPercentageFull = (kwhStored / maxKwhStored) * 100; // Update battery percentage
            }
        }

        private static double Min(params double[] values) {
            if (values == null || values.Length == 0) {
                throw new ArgumentException("Array cannot be null or empty.", nameof(values));
            }
            switch(values.Length) {
                case 1:
                    return values[0];
                case 2:
                    return Math.Min(values[0], values[1]);
                default:
                    break;
            }
            double min = values[0];
            for(int i = 1; i < values.Length; i++) {
                double value = values[i];
                if (value < min) {
                    min = value;
                }
            }
            return min;
        }
    }
}
