// Copyright (c) 2025 mvw684

using System;
using System.Collections.Generic;

using EnergiePrijzen.Config;
using EnergiePrijzen.Data.Apparaten;
using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Apparaten.ZonnePanelen;
using EnergiePrijzen.Data.Prijzen;
using EnergiePrijzen.Data.Report;

namespace EnergiePrijzen.Data {
    internal class DataGenerator {
        private readonly Settings settings;
        public DataGenerator(Settings settings) {
            this.settings = settings;
        }

        internal bool GenerateData() {
            var timeStamps = new UniqueItemList<TimeStamp>(EqualityComparer<TimeStamp>.Default);
            if (!settings.PeriodeStart.TryParseDate(out TimeStamp? start)) {
                return false;
            }
            if (!settings.PeriodeEnd.TryParseDate(out TimeStamp? end)) {
                return false;
            }
            TimeSpan duration = TimeStamp.Duration;
            // adding as universal time to filter out the daylight saving time gaps/jumps
            DateTime startDateTime = start.Value.Start.ToUniversalTime();
            DateTime ts = startDateTime;
            DateTime endDateTime = end.Value.Start.ToUniversalTime();
            while (ts < endDateTime) {
                timeStamps.Add(new TimeStamp(ts));
                ts += duration;
            }

            var inputData = new InputData { Settings = settings, Start = start.Value, End = end.Value, TimeStamps = timeStamps };

            var prijzen = new JeroenPrijzen(inputData);
            if(!prijzen.Load(out TimeStampedDataList<DynamischePrijs>? dynamischePrijzen)) {
                return false;
            }
            var panelenDataReader = new SolarEdgeReader(inputData);
            if (!panelenDataReader.Load(out TimeStampedDataList<ZonnePanelenProductie>? zonnepanelenProductie)) {
                return false;
            }
            var slimmeMeter = new SlimmeMeter(inputData);
            if (!slimmeMeter.Load(out TimeStampedDataList<MeterData>? meterData)) {
                return false;
            }

            if (!new ReportGenerator() { InputData = inputData, MeterData = meterData, Prijzen = dynamischePrijzen, PanelenData = zonnepanelenProductie }.Generate()) {
                return false;
            }
            return true;
        }
    }
}
