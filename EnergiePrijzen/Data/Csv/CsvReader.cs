// Copyright (c) 2025 mvw684

using System;
using System.IO;

using CH = CsvHelper;

namespace EnergiePrijzen.Data.Csv {
    
    
    internal class CsvReader : ReaderBase, IDisposable {

        private CH.CsvParser? parser;

        private readonly CH.Configuration.CsvConfiguration config;

        public CsvReader(FileInfo file, string delimiter) : base(file) {
            config = new CH.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
                Delimiter = delimiter,
                HasHeaderRecord = true,
                DetectDelimiter = false,
            };
            parser = new CH.CsvParser(new StreamReader(file.FullName), config, leaveOpen:false);
            if (parser.Read()) {
                CurrentRowNumber++;
                var header = parser.Record;
                if (header is not null) {
                    Header = header;
                }
            }
        }
        
        protected override bool ReadInternal(out string[] row) {
            if (parser is null) {
                throw new ObjectDisposedException($"Csv Reader {File.FullName} is disposed");
            }
            if (parser.Read()) {
                CurrentRowNumber++;
                var currentRowData = parser.Record;
                if (currentRowData != null) {
                    row = currentRowData;
                    return true;
                }
                row = Array.Empty<string>();
                return false;
            } else {
                row = Array.Empty<string>();
                return false;
            }
        }

        public void Dispose() {
            if (parser != null) {
                parser.Dispose();
                parser = null;
            }
        }
    }
}
