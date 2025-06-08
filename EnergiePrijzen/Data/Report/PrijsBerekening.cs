// Copyright (c) 2025 mvw684

using DocumentFormat.OpenXml.Drawing.Charts;

namespace EnergiePrijzen.Data.Report {
    internal class PrijsBerekening {

        // https://jeroen.nl/dynamische-energie/stroom/salderen-en-terugleveren#terugleververgoeding-op-basis-van-het-dynamisch-uurtarief

        private const double btw = 0.21; // 21% btw

        private const double afschrijvingBatterijPerKwh = 0.12; // 12 cent per kWh
        private const double stroomNetBeheerKostenPerDag = 1.30390;
        private const double stroomLeverKostenPerdag = 0.24625;
        private const double stroomInkoopVergoedingPerKwh = 0.02528;
        private const double stroomBelastingPerKwh = 0.12286;
        private const double kortingEnergiebelastingPerDag = 1.7405;

        private const double gasInkoopVergoedingPerM3 = 0.07872;
        private const double gasBelastingPerM3 = 0.69957;// 2024, 2025 --> 0.7 en 0.12
        private const double gasNetBeheerKostenPerDag = 0.67421;
        private const double gasLeverKostenPerDag = 0.19692;
        

        private readonly double stroomNetBeheerKostenPerTimeStamp = stroomNetBeheerKostenPerDag / (24 / TimeStamp.Duration.TotalHours);
        private readonly double stroomLeverKostenPerTimeStamp = stroomLeverKostenPerdag / (24 / TimeStamp.Duration.TotalHours);
        private readonly double kortingEnergiebelastingPerTimeStamp = kortingEnergiebelastingPerDag / (24 / TimeStamp.Duration.TotalHours);
        private readonly double gasNetBeheerKostenPerTimeStamp = gasNetBeheerKostenPerDag / (24 / TimeStamp.Duration.TotalHours);
        private readonly double gasLeverKostenPerTimeStamp = gasLeverKostenPerDag / (24 / TimeStamp.Duration.TotalHours);
        
        /// <summary>
        /// Pas de row aan met dynamische prijs/verbruik, leverkosten , belasting en andere details
        /// </summary>
        internal void Compute(ReportRowData data) {
            data.KwhKostenTotaal =
                (
                    (
                        ((data.KwhVerbruik - data.KwhTeruglevering) * (stroomBelastingPerKwh + data.KwhPrijs + stroomInkoopVergoedingPerKwh)) -
                        (stroomNetBeheerKostenPerTimeStamp + stroomLeverKostenPerTimeStamp - kortingEnergiebelastingPerTimeStamp)
                    ) * (1 + btw)
                ) + 
                (
                    data.KwhBatterijOntladen * afschrijvingBatterijPerKwh
                );

            data.M3KostenTotaal =
                (
                    (data.M3Gas * (gasBelastingPerM3 + data.M3Prijs + gasInkoopVergoedingPerM3)) +
                    (gasNetBeheerKostenPerTimeStamp + gasLeverKostenPerTimeStamp)
                ) * (1 + btw);

        }
    }
}
