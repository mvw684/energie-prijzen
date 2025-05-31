using System;

namespace EnergiePrijzen.Data.Report {
    internal class PrijsBerekening {

        // om en nabij getallen

        private const double btw = 0.21; // 21% btw
        private const double stroomNetBeheerKostenPerDag = 1.16656;
        private const double gasNetBeheerKostenPerDag = 0.61057;

        private const double gasLeverKostenPerdag = 0.21323;
        private const double stroomLeverKostenPerdag = 0.29477;

        private const double gasBelastingPerM3 = 0.71;// 2024, 2025 --> 0.7 en 0.12
        private const double stroomBelastingPerKwh = 0.13;

        private readonly double stroomNetBeheerKosenPerTimeStamp = stroomNetBeheerKostenPerDag / (24 * TimeStamp.Duration.TotalHours);
        private readonly double stroomLeverKostenPerTimeStamp = stroomLeverKostenPerdag / (24 * TimeStamp.Duration.TotalHours);
        private readonly double gasNetBeheerKosenPerTimeStamp = gasNetBeheerKostenPerDag / (24 * TimeStamp.Duration.TotalHours);
        private readonly double gasLeverKostenPerTimeStamp = gasLeverKostenPerdag / (24 * TimeStamp.Duration.TotalHours);
        
        /// <summary>
        /// Pas de row aan met dynamische prijs/verbruik, leverkosten , belasting en andere details
        /// </summary>
        internal void Compute(ReportRowData data) {
            data.KwhKostenTotaal =
                (
                    (data.KwhVerbruik * (stroomBelastingPerKwh + data.KwhPrijs)) + 
                    stroomNetBeheerKosenPerTimeStamp + stroomLeverKostenPerTimeStamp
                ) * (1 + btw);

            data.M3KostenTotaal =
                (
                    (data.M3Gas * (gasBelastingPerM3 + data.M3Prijs)) +
                    gasNetBeheerKosenPerTimeStamp + gasLeverKostenPerTimeStamp
                ) * (1 + btw);

        }
    }
}
