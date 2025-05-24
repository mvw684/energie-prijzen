// Copyright (c) 2025 mvw684

using System;
using System.IO;

using Excel = ClosedXML.Excel;

namespace EnergiePrijzen.Data.Csv {
    internal class ExcelWriter : IDisposable {
        private readonly FileInfo file;
        private Excel.XLWorkbook? workbook;
        private Excel.IXLWorksheet? sheet;
        private Excel.IXLRow? header;
        private int nrOfRows;

        public ExcelWriter(FileInfo file, string sheetName) {
            this.file = file;
            workbook = new Excel.XLWorkbook();
            sheet = workbook.Worksheets.Add(sheetName);
            nrOfRows = 1;
            header = sheet.Row(1);
        }

        public Excel.IXLRow Header {
            get => header ?? throw new ObjectDisposedException(file.FullName);
        }

        public Excel.IXLRow AddRow() {
            nrOfRows++;
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            var row = sheet.Row(++nrOfRows);
            return row;
        }

        public Excel.IXLColumn Column(int index) {
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            return sheet.Column(index);
        }

        public void Dispose() {
            if (workbook is not null) {
                sheet = null;
                header = null;
                workbook.SaveAs(file.FullName);
                workbook.Dispose();
                workbook = null;
            }
        }
    }
}
