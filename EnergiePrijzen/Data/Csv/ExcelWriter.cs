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
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            nrOfRows++;
            Excel.IXLRow row = sheet.Row(nrOfRows);
            return row;
        }

        public Excel.IXLColumn Column(int index) {
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            return sheet.Column(index);
        }

        public Excel.IXLRow Row(int index) {
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            return sheet.Row(index);
        }

        public void FreezeRows(int row) {
            if (sheet is null) {
                throw new ObjectDisposedException(file.FullName);
            }
            sheet.SheetView.FreezeRows(row); // Freeze the specified row
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
