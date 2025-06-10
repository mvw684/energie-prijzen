// Copyright (c) 2025 mvw684


using System;
using Excel = ClosedXML.Excel;
using EnergiePrijzen.Data.Csv;
using System.ComponentModel;


namespace EnergiePrijzen.Data.Report {
    
    internal class ReportRowData : TimeStampedDataBase<ReportRowData> {

        internal enum Field {

            [Description("Start")]
            Start = 1,

            [Description("End")]
            End,

            [Description("M3 Prijs")]
            M3Prijs,

            [Description("M3 Gas")]
            M3Gas,

            [Description("Gas €")]
            M3Kosten,

            [Description("Temperatuur")]
            Temperatuur,

            [Description("Kwh Prijs")]
            KwhPrijs,

            [Description("Kwh Verbruik")]
            KwhVerbruik,

            [Description("Kwh Teruglevering")]
            KwhTeruglevering,

            [Description("Kwh Zon Productie")]
            KwhZonProductie,

            [Description("Kwh Zon Direct Gebruik")]
            KwhZonDirectVerbruik,

            [Description("Kwh Totaal Gebruik")]
            KwhTotaalVerbruik,

            [Description("Kwh €")]
            KwhKosten,

            [Description("Kwh Batterij Stored")]
            KwhBatterijStored,

            [Description("Kwh Batterij Laden")]
            KwhBatterijLaden,

            [Description("Kwh Batterij Ontladen")]
            KwhBatterijOntladen,

            [Description("Batterij Percentage Full")]
            BatterijPercentageFull
        }

        private double kwhprijs;
        private double m3Prijs;
        private double m3gas;
        private double kwhVerbruik;
        private double kwhTeruglevering;
        private double kwhZonProductie;
        private double kwhZonDirectVerbruik;
        private double kwhTotaalVerbruik;

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

        public required double KwhZonProductie {
            get => kwhZonProductie;
            set => kwhZonProductie = value;
        }

        public required double KwhZonDirectVerbruik {
            get => kwhZonDirectVerbruik;
            set => kwhZonDirectVerbruik = value;
        }

        public required double KwhTotaalVerbruik {
            get => kwhTotaalVerbruik;
            set => kwhTotaalVerbruik = value;
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

            writer.Column(Field.Start).Style.DateFormat.Format = "dd-MM-yyyy HH:mm:ss";
            writer.Column(Field.End).Style.DateFormat.Format = "dd-MM-yyyy HH:mm:ss";
            writer.Column(Field.M3Prijs).Style.NumberFormat.Format = "€ #,##0.0000";
            writer.Column(Field.M3Gas).Style.NumberFormat.Format = "0.000";
            writer.Column(Field.M3Kosten).Style.NumberFormat.Format = "€ #,##0.00";
            writer.Column(Field.Temperatuur).Style.NumberFormat.Format = "0.00";

            writer.Column(Field.KwhPrijs).Style.NumberFormat.Format = "€ #,##0.0000";
            writer.Column(Field.KwhVerbruik).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhZonProductie).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhZonDirectVerbruik).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhTotaalVerbruik).Style.NumberFormat.Format = "#,##0.00";

            writer.Column(Field.KwhTeruglevering).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhKosten).Style.NumberFormat.Format = "€ #,##0.00";
            writer.Column(Field.KwhBatterijStored).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhBatterijLaden).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.KwhBatterijOntladen).Style.NumberFormat.Format = "#,##0.00";
            writer.Column(Field.BatterijPercentageFull).Style.NumberFormat.NumberFormatId = 10;

            foreach (Field field in Enum.GetValues<Field>()) {
                _ = writer.Column(field).AdjustToContents();
            }
            Excel.IXLRow header = writer.Header;
            _ = header.SetAutoFilter(); // Set auto filter on the header row
            _ = header.Style.Alignment.SetWrapText(true); // Wrap text in header row
            header.Style.Alignment.Vertical = Excel.XLAlignmentVerticalValues.Top;
            header.Style.Alignment.Horizontal = Excel.XLAlignmentHorizontalValues.Left;
            writer.FreezeRows(1); // Freeze the header row
        }                                                                           
                                                                                    
        internal static void WriteHeader(ExcelWriter writer) {                      
            Excel.IXLRow header = writer.Header;   
            foreach(Field field in Enum.GetValues<Field>()) {
                header.Cell(field.ToInt()).Value = field.GetHeaderValue();
            }
        }

        internal void WriteRow(ExcelWriter writer) {
            Excel.IXLRow row = writer.AddRow();           ;
            row.Cell(Field.Start).Value = TimeStamp.Start;
            row.Cell(Field.End).Value = TimeStamp.End;
            row.Cell(Field.M3Prijs).Value = M3Prijs;
            row.Cell(Field.M3Gas).Value = M3Gas;
            row.Cell(Field.M3Kosten).Value = M3KostenTotaal;
            row.Cell(Field.Temperatuur).Value = Temperatuur;


            row.Cell(Field.KwhPrijs).Value = KwhPrijs;
            row.Cell(Field.KwhVerbruik).Value = KwhVerbruik;
            row.Cell(Field.KwhTotaalVerbruik).Value = KwhTotaalVerbruik;
            row.Cell(Field.KwhZonProductie).Value = KwhZonProductie;
            row.Cell(Field.KwhZonDirectVerbruik).Value = KwhZonDirectVerbruik;
            row.Cell(Field.KwhTeruglevering).Value = KwhTeruglevering;
            row.Cell(Field.KwhKosten).Value = KwhKostenTotaal;
            row.Cell(Field.KwhBatterijStored).Value = KwhBatterijStored;
            row.Cell(Field.KwhBatterijLaden).Value = KwhBatterijLaden;
            row.Cell(Field.KwhBatterijOntladen).Value = KwhBatterijOntladen;
            row.Cell(Field.BatterijPercentageFull).Value = BatterijPercentageFull;
        }
    }
}
