// Copyright (c) 2025 mvw684


using System;
using Excel = ClosedXML.Excel;
using EnergiePrijzen.Data.Csv;


namespace EnergiePrijzen.Data.Report {
    
    internal class ReportRowData : TimeStampedDataBase<ReportRowData> {
        private double kwhprijs;
        private double m3Prijs;
        private double m3gas;
        private double kwhVerbruik;
        private double kwhTeruglevering;
        private double temperatuur;
        private double kwhBatterijLaden;
        private double kwhBatterijOntladen;
        private double kwhBatterijStored;
        private double batterijPercentageFull;

        private double kwhKostenTotaal;
        private double m3KostenTotaal;

        /// <summary>
        /// Prijs per kWh in euro's.
        /// </summary>
        public required double KwhPrijs {
            get => kwhprijs;
            init => kwhprijs = value;
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

        public double KwhKostenTotaal {
            get => kwhKostenTotaal;
            set => kwhKostenTotaal = value;
        }

        public double M3KostenTotaal {
            get => m3KostenTotaal;
            set => m3KostenTotaal = value;
        }

        internal static void Format(ExcelWriter writer) {
            int cell = 1;
            writer.Column(cell++).Style.DateFormat.Format = "dd-MM-yyyy HH:mm:ss";  //"Start";
            writer.Column(cell++).Style.DateFormat.Format = "dd-MM-yyyy HH:mm:ss";  //"End";
            writer.Column(cell++).Style.NumberFormat.Format = "€ #,##0.0000";         //"M3 Prijs";
            writer.Column(cell++).Style.NumberFormat.Format = "0.000";              //"M3 Gas";
            writer.Column(cell++).Style.NumberFormat.Format = "€ #,##0.00";         //"Gas €";
            writer.Column(cell++).Style.NumberFormat.Format = "0.00";               //"Temperatuur";

            writer.Column(cell++).Style.NumberFormat.Format = "€ #,##0.0000";         //"Kwh Prijs";
            writer.Column(cell++).Style.NumberFormat.Format = "#,##0.00";           //"Kwh Verbruik";
            writer.Column(cell++).Style.NumberFormat.Format = "#,##0.00";           // "Kwh Teruglevering";
            writer.Column(cell++).Style.NumberFormat.Format = "€ #,##0.00";         // "Kwh €";
            writer.Column(cell++).Style.NumberFormat.Format = "#,##0.00";           // "Kwh Batterij Stored";
            writer.Column(cell++).Style.NumberFormat.Format = "#,##0.00";           // "Kwh Batterij Laden";
            writer.Column(cell++).Style.NumberFormat.Format = "#,##0.00";           // "Kwh Batterij Ontladen";
            writer.Column(cell++).Style.NumberFormat.NumberFormatId = 10;           // "Batterij Percentage Full";

            cell = 1;
            writer.Column(cell++).AdjustToContents();  //"Start";
            writer.Column(cell++).AdjustToContents();  //"End";
            writer.Column(cell++).AdjustToContents();  //"M3 Prijs";
            writer.Column(cell++).AdjustToContents();  //"M3 Gas";
            writer.Column(cell++).AdjustToContents();  //"Gas €";
            writer.Column(cell++).AdjustToContents();  //"Temperatuur";

            writer.Column(cell++).AdjustToContents();  //"Kwh Prijs";
            writer.Column(cell++).AdjustToContents();  //"Kwh Verbruik";
            writer.Column(cell++).AdjustToContents();  // "Kwh Teruglevering";
            writer.Column(cell++).AdjustToContents();  // "Kwh €";
            writer.Column(cell++).AdjustToContents();  // "Kwh Batterij Stored";
            writer.Column(cell++).AdjustToContents();  // "Kwh Batterij Laden";
            writer.Column(cell++).AdjustToContents();  // "Kwh Batterij Ontladen";
            writer.Column(cell++).AdjustToContents();  // "Batterij Percentage Full";
            var header = writer.Header;
            header.SetAutoFilter(); // Set auto filter on the header row
            header.Style.Alignment.SetWrapText(true); // Wrap text in header row
            header.Style.Alignment.Vertical = Excel.XLAlignmentVerticalValues.Top;
            header.Style.Alignment.Horizontal = Excel.XLAlignmentHorizontalValues.Left;
            writer.FreezeRows(1); // Freeze the header row
        }                                                                           
                                                                                    
        internal static void WriteHeader(ExcelWriter writer) {                      
            Excel.IXLRow header = writer.Header;                                    
            int cell = 1;                                                           
            header.Cell(cell++).Value = "Start";
            header.Cell(cell++).Value = "End";
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
            Excel.IXLRow row = writer.AddRow();           ;
            int cell = 1;
            row.Cell(cell++).Value = TimeStamp.Start;
            row.Cell(cell++).Value = TimeStamp.End;
            row.Cell(cell++).Value = M3Prijs;
            row.Cell(cell++).Value = M3Gas;
            row.Cell(cell++).Value = M3KostenTotaal;
            row.Cell(cell++).Value = Temperatuur;


            row.Cell(cell++).Value = KwhPrijs;
            row.Cell(cell++).Value = KwhVerbruik;
            row.Cell(cell++).Value = KwhTeruglevering;
            row.Cell(cell++).Value = KwhKostenTotaal;
            row.Cell(cell++).Value = KwhBatterijStored;
            row.Cell(cell++).Value = KwhBatterijLaden;
            row.Cell(cell++).Value = KwhBatterijOntladen;
            row.Cell(cell++).Value = BatterijPercentageFull;
        }
    }
}
