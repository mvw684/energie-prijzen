// Copyright (c) 2025 mvw684

using System;

namespace EnergiePrijzen.Data.Report {
    internal class BatterySimulator {
        
        // laden/ontladen, ~20% verlies ( te berekenen via telemetry, kies een slecht getal voor simulatie)

        readonly int nrOfBatteries;
        readonly double maxKwhCharche;
        readonly double maxKwhDischarge;
        readonly double maxKwhStored;
        double kwhStored = 0;
        const double rendement = 0.9;

        internal BatterySimulator(int numberOfBatteries) {
            if (numberOfBatteries < 0) {
                throw new ArgumentOutOfRangeException(nameof(numberOfBatteries), "Number of batteries must be greater or equal than zero.");
            }
            nrOfBatteries = numberOfBatteries;
            maxKwhCharche = nrOfBatteries * 2200;
            maxKwhDischarge = nrOfBatteries * 1700;
            maxKwhStored = nrOfBatteries * 5000;
        }

        internal void Simulate(ReportRowData reportRow) {
            if (reportRow.KwhTeruglevering > 0) {
                // Battery charging
                double kwhToCharge = Min(reportRow.KwhTeruglevering, maxKwhCharche, (maxKwhStored - kwhStored) / rendement);
                double newKwhStored = kwhStored + (kwhToCharge * rendement);
                if (newKwhStored > maxKwhStored) {
                    kwhToCharge = (maxKwhStored - kwhStored) / rendement; // Limit to max storage capacity
                    newKwhStored = kwhStored + (kwhToCharge * rendement);
                }
                kwhStored += kwhToCharge * rendement;
                if (kwhStored > maxKwhStored) {
                    kwhStored = maxKwhStored; // Cap at maximum storage
                }
                reportRow.KwhBatterijLaden = kwhToCharge;
                reportRow.KwhTeruglevering -= kwhToCharge; // Reduce the amount of energy returned to the grid
                reportRow.KwhBatterijStored = kwhStored;
                double percentage = kwhStored / maxKwhStored;
                if (percentage > 1) {
                    percentage = 1; // Cap at 100%
                }
                reportRow.BatterijPercentageFull = percentage;

            } else if (reportRow.KwhVerbruik > 0) {
                // Battery discharging
                double kwhToDischarge = Min(reportRow.KwhVerbruik / rendement, maxKwhDischarge, kwhStored);
                kwhStored -= kwhToDischarge;
                reportRow.KwhBatterijOntladen = kwhToDischarge;
                reportRow.KwhVerbruik -= kwhToDischarge * rendement; // Reduce the amount of energy consumed from the grid
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
