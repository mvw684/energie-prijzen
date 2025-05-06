// Copyright (c) 2025 mvw684

using System;
using System.IO;

namespace EnergiePrijzen.Data.Csv {
    internal abstract class ReaderBase {

        private string[]? header;
        private string[]? currentRowData;
        private FileInfo file;
        private int currentRowNumber = 0;
        
        protected int CurrentRowNumber {  get  => currentRowNumber; set => currentRowNumber = value;}

        public ReaderBase(FileInfo file) {
            this.file = file;
        }

        public bool Hasheader => header != null;

        public string[] Header { 
            get => header ?? throw new InvalidOperationException("Header not read");
            set => header = value;
        }

        public FileInfo File => file;

        abstract protected bool ReadInternal(out string[] row);

        public bool Read(out string[] row) {

            if (ReadInternal(out row)) {
                currentRowData = row;
                return true;
            }
            row = Array.Empty<string>();
            return false;
        }

        internal InvalidDataException InvalidRow(string error) {
            var message = "Invalid row " + error + ", row data: " + string.Join(", ", currentRowData ?? Array.Empty<string>()) + "' in " + file.FullName + "@" + currentRowNumber;
            Tracer.Trace(message);
            return new InvalidDataException(message);
        }

        internal InvalidDataException Invalidheader(string error) {
            var message = "Invalid header '" + error + ", header: " + string.Join(", ", header ?? Array.Empty<string>()) + "' in " + file.FullName;
            Tracer.Trace(message);
            return new InvalidDataException(message);
        }

        internal void CheckHeader(params string[] expectedheader) {
            if (!Hasheader) {
                throw Invalidheader("missing header");
            }
            var header = Header;
            if (header.Length != expectedheader.Length) {
                throw Invalidheader($"expecting {expectedheader.Length} fields");
            }
            for (int i = 0; i < expectedheader.Length; i++) {
                if (header[i] != expectedheader[i]) {
                    throw Invalidheader($"expecting {string.Join(";", expectedheader)}, actual {string.Join(";", header)} ");
                }
            }
        }
    }
}
