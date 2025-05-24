// Copyright (c) 2025 mvw684


using System;

using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Report {
    
    internal class ReportRowData : TimeStampedDataBase<ReportRowData> {
        private double kwHprijs;
        private double m3Prijs;
        private double m3gas;
        private double kwhVerbruik;
        private double kwhTeruglevering;
        private double temperatuur;
        private double kwhBatterijLaden;
        private double kwhBatterijOntladen;
        private double kwhBatterijStored;
        private double batterijPercentageFull;

        /// <summary>
        /// Prijs per kWh in euro's.
        /// </summary>
        public required double KwHPrijs {
            get => kwHprijs;
            init => kwHprijs = value;
        }

        public required double M3Prijs {
            get => m3Prijs;
            init => m3Prijs = value;
        }

        public required double KwhVerbruik {
            get => kwhVerbruik;
            set => kwhVerbruik = value;
        }

        public required double KwhTeruglevering {
            get => kwhTeruglevering;
            set => kwhTeruglevering = value;
        }

        public required double M3Gas {
            get => m3gas;
            init => m3gas = value;
        }

        public required double Temperatuur {
            get => temperatuur;
            init => temperatuur = value;
        }

        public required double KwhBatterijLaden {
            get => kwhBatterijLaden;
            set => kwhBatterijLaden = value;
        }

        public required double KwhBatterijOntladen {
            get => kwhBatterijOntladen;
            set => kwhBatterijOntladen = value;
        }

        public required double BatterijPercentageFull {
            get => batterijPercentageFull;
            set => batterijPercentageFull = value;
        }

        public required double KwhBatterijStored {
            get => kwhBatterijStored;
            set => kwhBatterijStored = value;
        }

        internal static void Format(ExcelWriter writer) => throw new NotImplementedException();

        internal static void WriteHeader(ExcelWriter writer) {
            var header = writer.Header;
            int cell = 1;
            header.Cell(cell++).Value = "Timestamp";
            header.Cell(cell++).Value = "M3 Prijs";
            header.Cell(cell++).Value = "M3 Gas";
            header.Cell(cell++).Value = "Gas €";
            header.Cell(cell++).Value = "Temperatuur";
            

            header.Cell(cell++).Value = "Kwh Prijs";
            header.Cell(cell++).Value = "Kwh Verbruik";
            header.Cell(cell++).Value = "Kwh Teruglevering";
            header.Cell(cell++).Value = "Kwh €";
            header.Cell(cell++).Value = "Kwh Batterij Stored";
            header.Cell(cell++).Value = "Kwh Batterij Laden";
            header.Cell(cell++).Value = "Kwh Batterij Ontladen";
            header.Cell(cell++).Value = "Batterij Percentage Full";
        }

        internal void WriteRow(ExcelWriter writer) {
        }
    }
}
