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
        
        /// <summary>
        /// Pas de row aan met dynamische prijs/verbruik, leverkosten , belasting en andere details
        /// </summary>
        internal void Compute(ReportRowData data) {
            throw new NotImplementedException("Update method is not implemented yet.");
        }
    }
}
