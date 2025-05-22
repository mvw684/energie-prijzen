// Copyright (c) 2025 mvw684


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
    }
}
