using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;

namespace NTS.Document.Excel
{
    public class ExcelUtils<T>
    {
        public static void SetStyleExcel(IWorkbook workbook, IWorksheet sheet, List<T> listExport, int rowData, int countData, int columns, bool isAutoFitRows)
        {
            sheet.ImportData(listExport, rowData, 1, false);
            string columnName = GetExcelColumnName(columns);
            if (isAutoFitRows)
            {
                sheet.Range["A" + rowData + ":" + columnName + (rowData + countData)].AutofitRows();
            }

            IStyle bodyStyle = workbook.Styles.Add("BodyStyle");

            bodyStyle.BeginUpdate();
            bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

            bodyStyle.EndUpdate();

            sheet.Range["A" + rowData + ":" + columnName + (rowData + countData)].CellStyleName = "BodyStyle";
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
