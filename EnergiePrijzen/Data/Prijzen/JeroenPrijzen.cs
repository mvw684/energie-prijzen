// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace EnergiePrijzen.Data.Prijzen {
    internal class JeroenPrijzen {
        private readonly InputData inputData;
        private TimeStampedDataList<StroomPrijs> stroomPrijzen = new TimeStampedDataList<StroomPrijs>();
        private TimeStampedDataList<GasPrijs> gasPrijzen = new TimeStampedDataList<GasPrijs>();

        public JeroenPrijzen(InputData inputData) => this.inputData = inputData;

        internal bool Load([NotNullWhen(true)] out TimeStampedDataList<DynamischePrijs> dynamischePrijzen) {
            dynamischePrijzen = new TimeStampedDataList<DynamischePrijs>();
            stroomPrijzen = new TimeStampedDataList<StroomPrijs>();
            gasPrijzen = new TimeStampedDataList<GasPrijs>();


        var folder = new DirectoryInfo(inputData.Settings.JeroenPrijzen);
            bool result = true;
            foreach(var file in folder.EnumerateFiles("*stroomprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                result &= ReadStroomPrijzen(file);
            }

            foreach(var file in folder.EnumerateFiles("*gasprijzen*.csv", SearchOption.TopDirectoryOnly)) {
                result &= ReadGasPrijzen(file);
            }
            return result;
        }

        private bool ReadStroomPrijzen(FileInfo file) {
            using var reader = new StreamReader(file.FullName);
            string? line;
            while((line = reader.ReadLine()) != null) {
                if(line.StartsWith("Datum")) {
                    continue;
                }
                var parts = line.Split(';');
                if(parts.Length < 2) {
                    continue;
                }
                if(!DateTime.TryParse(parts[0], out DateTime date)) {
                    continue;
                }
                if(!double.TryParse(parts[1], out double price)) {
                    continue;
                }
                Console.WriteLine($"Date: {date}, Price: {price}");
            }
            stroomPrijzen.Add(new StroomPrijs { TimeStamp = new TimeStamp(), KwHPrijs = 0 });
            return false;
        }

        private bool ReadGasPrijzen(FileInfo file) {
            GC.KeepAlive(file);
            gasPrijzen.Add(new GasPrijs { TimeStamp = new TimeStamp(), M3Prijs = 0 });
            return false;
        }


    }
}
