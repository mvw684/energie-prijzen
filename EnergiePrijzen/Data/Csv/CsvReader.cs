// Copyright (c) 2025 mvw684

using System;
using System.IO;
using System.Linq;

using CH = CsvHelper;

namespace EnergiePrijzen.Data.Csv {
    
    
    internal class CsvReader : IDisposable {

        private readonly string[]? header;
        private string[]? currentRowData;
        private FileInfo file;
        private int currentRowNumber = 0;
        private CH.CsvParser? parser;

        private readonly CH.Configuration.CsvConfiguration config = new CH.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
            Delimiter = ";",
            HasHeaderRecord = true,
        };
        public CsvReader(FileInfo file, string delimiter) {
            this.file = file;
            config = new CH.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
                Delimiter = delimiter,
                HasHeaderRecord = true,
                DetectDelimiter = false,
            };
            parser = new CH.CsvParser(new StreamReader(file.FullName), config, leaveOpen:false);
            if (parser.Read()) {
                currentRowNumber++;
                header = parser.Record;
            }
        }

        public bool Hasheader => header != null;

        public string[] Header => header ?? throw new InvalidOperationException("Header not read");

        public bool Read(out string[] row) {
            if (parser is null) {
                throw new ObjectDisposedException($"Csv Reader {file.FullName} is disposed");
            }
            if (parser.Read()) {
                currentRowData = parser.Record;
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

        internal void ThrowInvalidRow(string error) {
            var message = "Invalid row "  + error + " " + string.Join(", ", currentRowData ?? Array.Empty<string>()) + "' in " + file.FullName + "@" + currentRowNumber;
            Tracer.Trace(message);
            throw new InvalidDataException(message);
        }

        internal void ThrowInvalidheader(string error){
            var message = "Invalid header '" + error + " " + string.Join(", ", header ?? Array.Empty<string>()) + "' in " + file.FullName;
            Tracer.Trace(message);
            throw new InvalidDataException(message);
        }

        internal void CheckHeader(params string[] expectedheader) {
            if (!Hasheader) {
                ThrowInvalidheader("missing header");
            }
            var header = Header;
            if (header.Length != expectedheader.Length) {
                ThrowInvalidheader($"expecting {expectedheader.Length} fields");
            }
            for (int i = 0; i < expectedheader.Length; i++) {
                if (header[i] != expectedheader[i]) {
                    ThrowInvalidheader($"expecting {string.Join(config.Delimiter, expectedheader)}, actual {string.Join(config.Delimiter, header)} ");
                }
            }
        }
    }
}
