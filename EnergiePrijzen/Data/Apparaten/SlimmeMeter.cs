// Copyright (c) 2025 mvw684

using System;
using System.IO;

using EnergiePrijzen.Data.Apparaten.Meter;
using EnergiePrijzen.Data.Csv;

namespace EnergiePrijzen.Data.Apparaten {
    internal class SlimmeMeter {
        private InputData inputData;

        public SlimmeMeter(InputData inputData) => this.inputData = inputData;

        public bool Load(out TimeStampedDataList<MeterData> meterData) {
            meterData = new TimeStampedDataList<MeterData>();
            var meterFolder = new DirectoryInfo(inputData.Settings.SlimmeMeter);
            bool result = ReadMeterData(meterFolder, meterData);
            return result;
        }

        private bool ReadMeterData(
            DirectoryInfo meterFolder, 
            TimeStampedDataList<MeterData> meterData
        ) {
            bool result = true;
            foreach (var fileInfo in meterFolder.EnumerateFiles("*.xls", SearchOption.TopDirectoryOnly)) {
                try {
                    using (var reader = new ExcelReader(fileInfo)) {

                    }
                } catch (Exception ex) {
                    Tracer.Trace($"Error reading file {fileInfo.FullName}: {ex.Message}");
                    result = false;
                }
            }
            return result;

        }
    }
}
