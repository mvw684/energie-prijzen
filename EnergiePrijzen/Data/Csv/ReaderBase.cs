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

        internal void ThrowInvalidRow(string error) {
            var message = "Invalid row " + error + " " + string.Join(", ", currentRowData ?? Array.Empty<string>()) + "' in " + file.FullName + "@" + currentRowNumber;
            Tracer.Trace(message);
            throw new InvalidDataException(message);
        }

        internal void ThrowInvalidheader(string error) {
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
                    ThrowInvalidheader($"expecting {string.Join(";", expectedheader)}, actual {string.Join(";", header)} ");
                }
            }
        }
    }
}
