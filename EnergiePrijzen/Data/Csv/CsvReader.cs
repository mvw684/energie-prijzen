// Copyright (c) 2025 mvw684

using System;
using System.IO;

using CH = CsvHelper;

namespace EnergiePrijzen.Data.Csv {
    
    
    internal class CsvReader : IDisposable {

        private readonly string[]? header; 
        private CH.CsvReader? reader;
        private readonly CH.Configuration.CsvConfiguration config = new CH.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
            Delimiter = ";",
            HasHeaderRecord = true,
        };
        public CsvReader(FileInfo file) {

            config = new CH.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
                Delimiter = ";",
                HasHeaderRecord = true,
                DetectDelimiter = false,
            };
            reader = new CH.CsvReader(new StreamReader(file.FullName), config);
            if (reader.Read() && reader.ReadHeader()) {
                header = reader.HeaderRecord;
            }
        }

        public bool Hasheader => header != null;

        public string[] Header => header ?? throw new InvalidOperationException("Header not read");

        public bool Read(out string[] row) {
            if (reader is null) {
                throw new InvalidOperationException("Reader is disposed");
            }
            if (reader.Read()) {
                row = reader.GetRecord<string[]>();
                return true;
            } else {
                row = Array.Empty<string>();
                return false;
            }
        }

        public void Dispose() {
            if (reader != null) {
                reader.Dispose();
                reader = null;
            }
        }
    }
}
