using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace NTS.Document.Excel
{
    public class ExcelService : IExcelService
    {
        public MemoryStream ExportExcel<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];

                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);


                    workbook.SaveAs(stream);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return stream;
        }

        public MemoryStream ExportExcel<T>(List<ItemDataSheet<T>> dataSheets, string teamplatePath, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                IWorkbook workbook = null;
                try
                {
                    //IApplication application = excelEngine.Excel;
                    //FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                     workbook = application.Workbooks.Open(inputStream);

                    IWorksheet sheet = null;
                    foreach (var itemSheet in dataSheets)
                    {
                        sheet = workbook.Worksheets[itemSheet.SheetIndex];

                        ExportData(workbook, sheet, itemSheet.Datas, teamplatePath, true, itemSheet.Columns, paramDic);
                    }
                    workbook.SaveAs(stream);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    workbook?.Close();
                }
            }

            return stream;
        }

        /// <summary>
        /// Xuất dữ liệu ra excel trả vè base64
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu xuất</typeparam>
        /// <param name="datas">Dữ liệu xuất</param>
        /// <param name="teamplatePath">Đường dẫn file mẫu</param>
        /// <param name="columns">Cột cần autofit</param>
        /// <param name="paramDic">Danh sách param cần replace</param>
        /// <returns></returns>
        public string ExportExcelBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            string base64String = string.Empty;
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        base64String = Convert.ToBase64String(stream.ToArray());
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }
            return base64String;
        }

        /// <summary>
        /// Xuất dữ liệu ra excel trả  về file
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu xuất</typeparam>
        /// <param name="datas">Dữ liệu xuất</param>
        /// <param name="teamplatePath">Đường dẫn file mẫu</param>
        /// <param name="columns">Cột cần autofit</param>
        /// <param name="paramDic">Danh sách param cần replace</param>
        /// <returns></returns>
        public DocumentResultModel ExportExcelFile<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            DocumentResultModel fileResultModel = new DocumentResultModel();
            fileResultModel.ContentType = FileHelper.GetContentType(".xlsx");
            fileResultModel.FileName = Path.GetFileName(teamplatePath);

            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                try
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        fileResultModel.FileStream = stream.ToArray();
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    inputStream.Close();

                }

                excelEngine.Dispose();
            }

            return fileResultModel;
        }

        public DocumentResultModel ExportExcelMultiSheetFile<T>(List<List<T>> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            DocumentResultModel fileResultModel = new DocumentResultModel();
            fileResultModel.ContentType = FileHelper.GetContentType(".xlsx");
            fileResultModel.FileName = Path.GetFileName(teamplatePath);

            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                try
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(inputStream);

                    int index = 0;
                    IWorksheet sheet;
                    foreach (var data in datas)
                    {
                        sheet = workbook.Worksheets[index];
                        ExportData(workbook, sheet, data, teamplatePath, true, columns, paramDic);
                        index++;
                    }


                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        fileResultModel.FileStream = stream.ToArray();
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    inputStream.Close();

                }

                excelEngine.Dispose();
            }

            return fileResultModel;
        }

        public MemoryStream ExportPdf<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();
            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    //Initialize XlsIORendererSettings
                    XlsIORendererSettings settings = new XlsIORendererSettings();

                    //Enable AutoDetectComplexScript property
                    settings.AutoDetectComplexScript = true;
                    settings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;

                    XlsIORenderer render = new XlsIORenderer();

                    PdfDocument pdfDocument = render.ConvertToPDF(workbook.Worksheets[0], settings);



                    pdfDocument.Save(stream);

                    pdfDocument.Close(true);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return stream;
        }

        public string ExportPdfBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            string base64String = string.Empty;
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {

                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    XlsIORenderer render = new XlsIORenderer();
                    PdfDocument pdfDocument = render.ConvertToPDF(workbook.Worksheets[0]);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfDocument.Save(stream);
                        base64String = Convert.ToBase64String(stream.ToArray());
                        stream.Dispose();
                    }

                    pdfDocument.Close(true);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return base64String;
        }

        private void ExportData<T>(IWorkbook workbook, IWorksheet sheet, List<T> datas, string teamplatePath, bool autofitRows, int columns, Dictionary<string, string> paramDic = null)
        {
            if (paramDic != null)
            {
                IRange range;
                foreach (KeyValuePair<string, string> item in paramDic)
                {
                    range = sheet.FindFirst(item.Key, ExcelFindType.Text, ExcelFindOptions.MatchCase);

                    if (range != null)
                    {
                        range.Replace(item.Key, item.Value != null ? item.Value.ToString() : "");
                    }
                }
            }

            int total = datas.Count;

            IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            if (iRangeData == null)
            {
                return;
            }

            iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

            int rowData = iRangeData.Row;
            int columnData = iRangeData.Column;
            List<string> cplumnExports = new List<string>();
            int index = 0;
            string cellValue;
            bool isExist = false;
            int cellIndex = 0;
            while (sheet.Rows.Length > rowData)
            {
                cellIndex = columnData + index - 1;
                if (sheet.Rows[rowData].Cells.Length <= cellIndex)
                {
                    break;
                }

                cellValue = sheet.Rows[rowData].Cells[cellIndex].Value;

                if (!string.IsNullOrEmpty(cellValue))
                {

                    cplumnExports.Add(cellValue.Replace("<", string.Empty).Replace(">", string.Empty));
                    isExist = true;
                    sheet.Rows[rowData].Cells[cellIndex].Value = string.Empty;
                }
                else
                {
                    break;
                }

                index++;
            }

            if (total == 0)
            {
                sheet.DeleteRow(rowData);
            }

            if (total > 1)
            {
                sheet.InsertRow(rowData + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
            }

            if (isExist)
            {
                DataTable dtb = new DataTable();

                var exportData = datas.ToDataTableExport(cplumnExports);

                sheet.ImportDataTable(exportData, rowData, columnData, false, false);
            }
            else
            {
                sheet.ImportData(datas, rowData, columnData, false);
            }

            // Autofit dòng đầu tiên của Data khi import, thường khi xuất Pdf thì dòng đầu tiên đang không autofit
            string columnName = GetExcelColumnName(columns);
            if (autofitRows && total > 0)
            {
                sheet.Range["A" + rowData + ":" + columnName + (rowData + total - 1)].AutofitRows();
            }

            //SetBorder(workbook);

            //sheet.Range["A" + rowData + ":" + columnName + (rowData + total - 1)].CellStyleName = "BodyStyle";
        }

        //private void SetBorder(IWorkbook workbook)
        //{
        //    IStyle bodyStyle = workbook.Styles.Add("BodyStyle");

        //    bodyStyle.BeginUpdate();
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

        //    bodyStyle.EndUpdate();
        //}

        private string GetExcelColumnName(int columnNumber)
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

        /// <summary>
        /// Chuyển đổi file .xls sang file .pdf
        /// </summary>
        /// <param name="pathXLS">Đường dẫn file excel đầu vào</param>
        /// <param name="pathOutPdf">Đường dẫn file .pdf đầu ra</param>
        /// <returns></returns>
        public FileStream ConvertToPDF(string pathXLS, string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathXLS))
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0013);

                FileStream fileStreamPath = new FileStream(@$"{pathXLS}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XlsIORenderer render = new XlsIORenderer();
                var pdfDocument = render.ConvertToPDF(fileStreamPath);

                FileStream outputStream = new FileStream(pathOutPdf, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                pdfDocument.Save(outputStream);
                //Closes the instance of PDF document object.
                pdfDocument.Close();
                //Dispose the instance of FileStream.
                return outputStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xuất excel từ file mẫu theeo các thông tin trong object truyền vào
        /// Lưu ý định nghĩa các trường thông tin trên file word giống với các trường trong model và nằm trong dấu <> 
        /// </summary>
        /// <typeparam name="T">Kiểu object truyền vào</typeparam>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="model">Model đâu thông tin cần xuất ra file excel</param>
        /// <returns></returns>
        public MemoryStream ExportExcel<T>(string templatePath, T model)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), templatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];

                    // Lấy phạm vi sử dụng của trang tính
                    IRange usedRange = sheet.UsedRange;

                    var propertyModel = model.GetType().GetProperties();
                    IRange[] ranges;
                    string name, value, keyTable;
                    IList listValue;
                    foreach (var itemPropModel in propertyModel)
                    {
                        if (itemPropModel.IsListProperty())//Nếu là list
                        {
                            keyTable = itemPropModel.Name;
                            listValue = (IList)itemPropModel.GetValue(model, null);
                            //Tìm vùng cần chèn dữ liệu bảng vào
                            IRange iRangeTable = sheet.FindFirst($"<{keyTable}>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                            if (iRangeTable != null)
                            {
                                //DÒng đầu tiên để chèn dữ liệu bảng
                                int rowFirst = iRangeTable.Row;
                                int cellMax = this.GetCellMax(sheet, rowFirst);
                                //Xóa key tabale tại dòng tìm được
                                iRangeTable.Text = iRangeTable.Text.Replace($"<{keyTable}>", string.Empty);
                                if (listValue?.Count > 0)
                                {
                                    //Tạo ra tổng số dòng có Format giống dòng cần chèn dữ liệu vào
                                    sheet.InsertRow(rowFirst + 1, listValue.Count, ExcelInsertOptions.FormatAsBefore);
                                    string nameCell, valueCell;
                                    int indexRow = rowFirst + 1;
                                    foreach (var item in listValue)
                                    {
                                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                                        {
                                            nameCell = prop.Name;
                                            valueCell = prop.GetValue(item, null)?.ToString() ?? "";
                                            // Thiết lập giá trị cho từng ô trong hàng
                                            for (int i = 1; i <= cellMax; i++)
                                            {
                                                if (sheet.GetText(rowFirst, i).Equals($"<{nameCell}>"))
                                                    sheet.SetText(indexRow, i, valueCell);
                                            }
                                        }
                                        indexRow++;
                                    }
                                    sheet.DeleteRow(rowFirst);
                                }
                                else
                                {
                                    // Xóa giá trị trong dòng
                                    iRangeTable.Clear();
                                }
                            }
                        }
                        else
                        {
                            name = itemPropModel.Name;
                            value = itemPropModel.GetValue(model, null)?.ToString() ?? "";
                            ranges = sheet.FindAll($"<{name}>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                            if (ranges != null && ranges.Count() > 0)
                            {
                                foreach (var itemRange in ranges)
                                {
                                    itemRange.Replace($"<{name}>", value);
                                }
                            }
                        }
                    }

                    workbook.SaveAs(stream);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return stream;
        }


        /// <summary>
        ///  Export excel to pdf Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="model">Model data</param>
        public MemoryStream ExportExcelConvertToPdf<T>(string templatePath, T model)
        {
            MemoryStream outputStream = new MemoryStream();
            PdfDocument pdfDocument = null;
            try
            {
                var streamWord = ExportExcel<T>(templatePath, model);
                DocIORenderer render = new DocIORenderer();
                pdfDocument = render.ConvertToPDF(streamWord);
                render.Dispose();
                pdfDocument.Save(outputStream);
                //Closes the instance of PDF document object.
                pdfDocument.Close();
                //Dispose the instance of FileStream.
                outputStream.Dispose();
                return outputStream;
            }
            catch
            {
                pdfDocument?.Close();
                return outputStream;
            }

        }

        /// <summary>
        /// Lấy vị trí kết thúc của cell file dữ liệu bảng
        /// </summary>
        /// <param name="sheet">Sheet dữ liệu</param>
        /// <param name="rowFirst">Dòng đầu tiên fill dữ liệu</param>
        /// <returns></returns>
        private int GetCellMax(IWorksheet sheet, int rowFirst)
        {
            int cellMax = 0;
            // Lấy tất cả các dòng trong phạm vi sử dụng của trang tính
            IRange usedRange = sheet.UsedRange;

            IRange rangeData = usedRange.Rows[rowFirst - 1];
            if (rangeData != null)
            {
                // Lặp qua từng ô trong dòng và kiểm tra xem ô có dữ liệu hay không
                foreach (IRange cell in rangeData.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.Value) && cell.Column > cellMax)
                    {
                        cellMax = cell.Column;
                    }
                }
            }

            return cellMax;
        }
    }
}
