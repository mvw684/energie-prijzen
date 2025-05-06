// Copyright (c) 2025 mvw684

using System;
using System.IO;
using System.Linq;

using DocumentFormat.OpenXml.Spreadsheet;

using Excel = ClosedXML.Excel;

namespace EnergiePrijzen.Data.Csv {

    /// <summary>
    /// Simple reader assuming ONE sheet per file
    /// </summary>
    internal class ExcelReader : ReaderBase, IDisposable {
        Excel.XLWorkbook? workbook;
        Excel.IXLWorksheet? sheet;
        public ExcelReader(FileInfo file) : base(file) {
            workbook = new Excel.XLWorkbook(file.FullName);
            if (workbook.Worksheets.Count > 1) {
                throw new InvalidDataException($"Excel file {file.FullName} has more than one sheet");
            }
            sheet = workbook.Worksheet(1);
            var headerRow = sheet.FirstRow();
            if (headerRow is not null) {
                var header = GetStringValues(headerRow);
                Header = header;
                CurrentRowNumber = 1;
            }
        }

        protected override bool ReadInternal(out string[] row) {
            if (workbook is null) {
                throw new ObjectDisposedException($"Excel Reader {File.FullName} is disposed");
            }
            if (CurrentRowNumber + 1 > sheet?.RowCount()) {
                row = Array.Empty<string>();
                return false;
            }
            CurrentRowNumber++;
            var excelRow = sheet?.Row(CurrentRowNumber);
            if (excelRow is not null) {
                row = GetStringValues(excelRow);
                return true;
            } else {
                row = Array.Empty<string>();
                return false;
            }
        }

        private string[] GetStringValues(Excel.IXLRow excelRow) {
            
            var used = excelRow.CellsUsed();
            var lastUsed = used.LastOrDefault();
            var column = lastUsed?.Address.ColumnNumber ?? 0;
            var cellCount = column;
            var row = new string[cellCount];
            for (int i = 0; i < cellCount; i++) {
                var cell = excelRow.Cell(i + 1);
                if (cell != null) {
                    row[i] = cell.GetString();
                } else {
                    row[i] = string.Empty;
                }
            }
            return row;
        }

        public void Dispose() {
            workbook?.Dispose();
            workbook = null;
        }


    }
}
