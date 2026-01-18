//using NTS.Common;
//using NTS.Common.Resource;
//using Syncfusion.DocIO;
//using Syncfusion.DocIO.DLS;
//using Syncfusion.DocIORenderer;
//using Syncfusion.Pdf;
//using System.Reflection;

using NTS.Common;
using NTS.Common.Resource;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NTS.Document.Word
{
    public class WordService : IWordService
    {
        /// <summary>
        /// Export word Out MemoryStream
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        /// <returns></returns>
        public MemoryStream ExportWord(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            MemoryStream stream = new MemoryStream();

            if (!File.Exists(templatePath))
                return stream;

            using (WordDocument document = new WordDocument())
            {
                try
                {
                    FileStream fileStreamPath = new FileStream(@$"{templatePath}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    document.Open(fileStreamPath, FormatType.Automatic);

                    //Replace text, html in template
                    if (replateTexts?.Count > 0)
                    {
                        foreach (ItemTextReplate item in replateTexts)
                        {
                            document.ReplaceItem(item);
                        }
                    }

                    //Replace image in template
                    if (replateImages?.Count > 0)
                    {
                        foreach (ItemImageReplate item in replateImages)
                        {
                            document.ReplaceImage(item);
                        }
                    }

                    //Fill data to table
                    if (dataTables?.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<object>> itemDataTable in dataTables)
                        {
                            WTable tableIndex;
                            string keyTable = itemDataTable.Key;
                            tableIndex = document.GetTableByFindText(keyTable);
                            //Row bắt đầu fill data
                            WTableRow tableRowData = document.GetTableRowByFindText(keyTable);
                            WTableCell tableCellData = document.GetTableCellByFindText(keyTable);
                            if (tableIndex != null)
                            {
                                int indexRow = tableRowData.GetRowIndex();
                                //Xóa row data dầu tiên
                                tableIndex.Rows.RemoveAt(indexRow);
                                if (dataTables?.Count > 0)
                                {
                                    WTableRow row;
                                    foreach (var item in itemDataTable.Value)
                                    {
                                        tableIndex.Rows.Insert(indexRow, tableRowData.Clone());

                                        row = tableIndex.Rows[indexRow];

                                        int indexCell = tableCellData.GetCellIndex();
                                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                                        {
                                            if (indexCell < row.Cells.Count)
                                            {
                                                row.Cells[indexCell].Paragraphs[0].Text = prop.GetValue(item, null) != null ? prop.GetValue(item, null).ToString() : string.Empty;
                                                indexCell++;
                                            }
                                        }

                                        indexRow++;
                                    }
                                }
                                else
                                {
                                    document.Sections[0].Body.ChildEntities.Remove(tableIndex);
                                }
                            }
                        }
                    }
                    //Saves the document to stream
                    document.Save(stream, FormatType.Docx);
                    //Closes the document
                    document.Close();
                }
                catch (Exception ex)
                {
                    //Closes the document
                    document?.Close();
                }
            }

            return stream;
        }

        /// <summary>
        ///  Export word Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        public void ExportWord(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            var streamWord = ExportWord(templatePath, replateTexts, dataTables, replateImages);
            FileStream file = null;
            try
            {
                using (file = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    streamWord.Position = 0;
                    streamWord.CopyTo(file);
                    file.Close();
                }
            }
            catch { file?.Close(); }
        }

        /// <summary>
        /// Export word to pdf Out MemoryStream
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="replateTexts"></param>
        /// <param name="dataTables"></param>
        /// <param name="replateImages"></param>
        /// <returns></returns>
        public MemoryStream ExportWordConvertToPdf(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            MemoryStream outputStream = new MemoryStream();
            PdfDocument pdfDocument = null;
            try
            {
                var streamWord = ExportWord(templatePath, replateTexts, dataTables, replateImages);
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
        ///  Export word to pdf Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        public void ExportWordConvertToPdf(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            try
            {
                var streamWord = ExportWordConvertToPdf(templatePath, replateTexts, dataTables, replateImages);
                using (FileStream file = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    streamWord.Position = 0;
                    streamWord.CopyTo(file);
                    file.Close();
                }
            }
            catch { }
        }

        public MemoryStream ImportWordContent(MemoryStream streamSource, MemoryStream streamImport)
        {
            MemoryStream streamOutput = new MemoryStream();
            if (streamSource == null || streamImport == null)
                return null;
            try
            {
                using (WordDocument documentSource = new WordDocument())
                {
                    documentSource.Open(streamSource, FormatType.Automatic);
                    using (WordDocument documentImport = new WordDocument())
                    {
                        documentImport.Open(streamImport, FormatType.Automatic);

                        documentSource.ImportContent(documentImport);
                        documentImport.Close();
                    }
                    //Saves the document to stream
                    documentSource.Save(streamOutput, FormatType.Docx);
                    //Closes the document
                    documentSource.Close();
                    streamOutput.Position = 0;
                }
            }
            catch { }
            return streamOutput;
        }

        /// <summary>
        /// Chuyển đổi file word sang pdf
        /// </summary>
        /// <param name="pathDoc">Đường dẫn file .doc đầu vào</param>
        /// <param name="pathOutPdf">Đường dẫn file pdf đầu ra</param>
        /// <returns></returns>
        public FileStream ConvertToPDF(string pathDoc, string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathDoc))
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0013);

                FileStream fileStreamPath = new FileStream(@$"{pathDoc}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fileStreamPath.Position = 0;
                DocIORenderer render = new DocIORenderer();
                var pdfDocument = render.ConvertToPDF(fileStreamPath);
                render.Dispose();

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
        /// Xuất word từ file mẫu theeo các thông tin trong object truyền vào
        /// Lưu ý định nghĩa các trường thông tin trên file word giống với các trường trong model và nằm trong dấu <> 
        /// </summary>
        /// <typeparam name="T">Kiểu object truyền vào</typeparam>
        /// <param name="templatePath">Dường dẫn file template</param>
        /// <param name="model">Model đâu thông tin cần xuất ra file word</param>
        /// <returns></returns>
        public MemoryStream ExportWord<T>(string templatePath, T model)
        {
            MemoryStream stream = new MemoryStream();

            if (!File.Exists(templatePath))
                return stream;

            using (WordDocument document = new WordDocument())
            {
                try
                {
                    FileStream fileStreamPath = new FileStream(@$"{templatePath}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    document.Open(fileStreamPath, FormatType.Automatic);

                    var propertyModel = model.GetType().GetProperties();
                    string name, value, keyTable;
                    IList listValue;
                    foreach (var itemPropModel in propertyModel)
                    {
                        if (itemPropModel.IsListProperty())//Nếu là list
                        {
                            keyTable = itemPropModel.Name;
                            listValue = (IList)itemPropModel.GetValue(model, null);
                            WTable tableIndex = document.GetTableByFindText($"<{keyTable}>");
                            //Row bắt đầu fill data
                            WTableRow tableRowData = document.GetTableRowByFindText($"<{keyTable}>").RemoveKeyTableRow($"<{keyTable}>");
                            if (tableIndex != null)
                            {
                                int indexRow = tableRowData.GetRowIndex();
                                //Xóa dòng cấu hình tên trường
                                tableIndex.Rows.RemoveAt(indexRow);
                                if (listValue?.Count > 0)
                                {
                                    WTableRow row;
                                    string nameCell, valueCell;
                                    foreach (var item in listValue)
                                    {
                                        tableIndex.Rows.Insert(indexRow, tableRowData.Clone());

                                        row = tableIndex.Rows[indexRow];
                                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                                        {
                                            nameCell = prop.Name;
                                            valueCell = prop.GetValue(item, null)?.ToString() ?? "";
                                            // Thiết lập giá trị cho từng ô trong hàng
                                            for (int i = 0; i < row.Cells.Count; i++)
                                            {
                                                if (row.Cells[i].Paragraphs[0].Text.Equals($"<{nameCell}>"))
                                                    row.Cells[i].Paragraphs[0].Text = valueCell;
                                            }
                                        }

                                        indexRow++;
                                    }
                                }
                                else
                                {
                                    //Xóa giá trị cấu hình dòng table
                                    document.Sections[0].Body.ChildEntities.Remove(tableIndex);
                                }
                            }
                        }
                        else
                        {
                            name = itemPropModel.Name;
                            value = itemPropModel.GetValue(model, null)?.ToString() ?? "";
                            document.Replace($"<{name}>", value, false, true);
                        }
                    }

                    //Saves the document to stream
                    document.Save(stream, FormatType.Docx);
                    //Closes the document
                    document.Close();
                }
                catch (Exception ex)
                {
                    //Closes the document
                    document?.Close();
                }
            }

            return stream;
        }


        /// <summary>
        ///  Export word to pdf Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="model">Model data</param>
        public MemoryStream ExportWordConvertToPdf<T>(string templatePath, T model)
        {
            MemoryStream outputStream = new MemoryStream();
            PdfDocument pdfDocument = null;
            try
            {
                var streamWord = ExportWord<T>(templatePath, model);
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
    }
}
