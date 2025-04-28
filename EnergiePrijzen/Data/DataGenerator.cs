// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

using EnergiePrijzen.Config;
using EnergiePrijzen.Data.Prijzen;

namespace EnergiePrijzen.Data {
    internal class DataGenerator {
        private readonly Settings settings;
        
        public DataGenerator(Settings settings) => this.settings = settings;

        internal bool GenerateData() {
            var timeStamps = new UniqueItemList<TimeStamp>(EqualityComparer<TimeStamp>.Default);
            if (!settings.PeriodeStart.TryParseDate(out var start)) {
                return false;
            }
            if (!settings.PeriodeEnd.TryParseDate(out var end)) {
                return false;
            }

            TimeSpan duration = TimeStamp.Duration;
            var ts = start;
            while(ts < end) {
                timeStamps.Add(ts.Value);
                ts += duration;
            }

            var inputData = new InputData { Settings = settings, Start = start.Value, End = end.Value, TimeStamps = timeStamps };

            var prijzen = new JeroenPrijzen(inputData);
            if(!prijzen.Load(out var dynamischePrijzen)) {
                return false;
            }
            foreach (var prijs in dynamischePrijzen) {
                Console.WriteLine($"Prijs: {prijs}");
            }
            return true;
        }
    }
}
