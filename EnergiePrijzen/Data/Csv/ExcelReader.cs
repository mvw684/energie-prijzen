// Copyright (c) 2025 mvw684

using System;
using System.IO;
using System.Linq;

using Excel = ClosedXML.Excel;

namespace EnergiePrijzen.Data.Csv {

    /// <summary>
    /// Simple reader assuming ONE sheet per file
    /// </summary>
    internal class ExcelReader : ReaderBase, IDisposable {
        private Excel.XLWorkbook? workbook;
        private Excel.IXLWorksheet? sheet;
        private readonly int nrOfRows;
        public ExcelReader(FileInfo file) : base(file) {
            workbook = new Excel.XLWorkbook(file.FullName);
            if (workbook.Worksheets.Count > 1) {
                throw new InvalidDataException($"Excel file {file.FullName} has more than one sheet");
            }
            sheet = workbook.Worksheet(1);
            Excel.IXLRow? headerRow = sheet.FirstRow();
            nrOfRows = sheet.RowsUsed().Count();
            if (headerRow is not null) {
                string[] header = GetStringValues(headerRow);
                Header = header;
                CurrentRowNumber = 1;
            }
        }

        protected override bool ReadInternal(out string[] row) {
            if (workbook is null) {
                throw new ObjectDisposedException($"Excel Reader {File.FullName} is disposed");
            }
            if (CurrentRowNumber >= nrOfRows) {
                row = Array.Empty<string>();
                return false;
            }
            CurrentRowNumber++;
            Excel.IXLRow? excelRow = sheet?.Row(CurrentRowNumber);
            if (excelRow is not null) {
                row = GetStringValues(excelRow);
                return true;
            } else {
                row = Array.Empty<string>();
                return false;
            }
        }

        private static string[] GetStringValues(Excel.IXLRow excelRow) {

            Excel.IXLCells used = excelRow.CellsUsed();
            Excel.IXLCell? lastUsed = used.LastOrDefault();
            int column = lastUsed?.Address.ColumnNumber ?? 0;
            int cellCount = column;
            string[] row = new string[cellCount];
            for (int i = 0; i < cellCount; i++) {
                Excel.IXLCell cell = excelRow.Cell(i + 1);
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
            sheet = null;
        }


    }
}
