using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NTS.Document.Word
{
    public static class WordUtils
    {
        public static WTable GetTableByFindText(this WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTable))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTable)
            {
                return entity as WTable;
            }
            else
            {
                return null;
            }
        }

        public static WTableRow GetTableRowByFindText(this WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTableRow))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTableRow)
            {
                return entity as WTableRow;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Xóa key trong row
        /// </summary>
        /// <param name="tableRow"></param>
        /// <param name="keyRemove"></param>
        public static WTableRow RemoveKeyTableRow(this WTableRow tableRow, string keyRemove)
        {
            if (tableRow != null)
            {
                for (int i = 0; i < tableRow.Cells.Count; i++)
                {
                    if (tableRow.Cells[i].Paragraphs[0].Text.Contains(keyRemove))
                        tableRow.Cells[i].Paragraphs[0].Text = tableRow.Cells[i].Paragraphs[0].Text.Replace(keyRemove, "");
                }
            }
            return tableRow;
        }

        public static WTableCell GetTableCellByFindText(this WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTableCell))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTableCell)
            {
                return entity as WTableCell;
            }
            else
            {
                return null;
            }
        }

        public static string NTSOverHtmlString(this string stringData)
        {

            //string cleaned = new Regex("style=\"[^\"]*\"").Replace(stringData, "");
            // cleaned = new Regex("(?<=class=\")([^\"]*)\\babc\\w*\\b([^\"]*)(?=\")").Replace(cleaned, "$1$2");
            //cleaned = cleaned.Replace("<span", "<span style=\"font-size: 14.0pt; font-family: 'Times New Roman';\"");
            if (!string.IsNullOrEmpty(stringData))
            {
                List<int> listIndex = new List<int>();
                for (int i = 0; i < stringData.Length; i++)
                {
                    if (i + 1 < stringData.Length && stringData[i].Equals('<') && stringData[i + 1].Equals('p'))
                    {
                        int indexSty = stringData.IndexOf("style=\"", i + 1);

                        if (indexSty != -1)
                        {
                            for (int j = indexSty + 7; j < stringData.Length; j++)
                            {
                                if (stringData[j].Equals('"'))
                                {
                                    listIndex.Add(j);
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = listIndex.Count - 1; i >= 0; i--)
                {
                    stringData = stringData.Insert(listIndex[i], "line-height: 130%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';");
                }

                if (stringData.Contains("<p>"))
                {
                    stringData = stringData.Replace("<p>", "<p style=\"line-height: 130%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';\">");
                }

                string ret = new Regex(@"line-height.+?;").Replace(stringData, "line-height: 130%;");
                ret = new Regex(@"mso-margin-top-alt.+?;").Replace(ret, string.Empty);
                ret = new Regex(@"mso-margin-bottom-alt.+?;").Replace(ret, string.Empty);
                return ret;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringData"></param>
        /// <param name="lineSpacing">Cách dòng (VD: 1,3 = 130)</param>
        /// <returns></returns>
        public static string NTSOverHtmlString(this string stringData, int lineSpacing, bool isCenter = false)
        {

            //string cleaned = new Regex("style=\"[^\"]*\"").Replace(stringData, "");
            // cleaned = new Regex("(?<=class=\")([^\"]*)\\babc\\w*\\b([^\"]*)(?=\")").Replace(cleaned, "$1$2");
            //cleaned = cleaned.Replace("<span", "<span style=\"font-size: 14.0pt; font-family: 'Times New Roman';\"");
            if (!string.IsNullOrEmpty(stringData))
            {

                List<int> listIndex = new List<int>();
                for (int i = 0; i < stringData.Length; i++)
                {
                    if (i + 1 < stringData.Length && stringData[i].Equals('<') && stringData[i + 1].Equals('p'))
                    {
                        int indexSty = stringData.IndexOf("style=\"", i + 1);

                        if (indexSty != -1)
                        {
                            for (int j = indexSty + 7; j < stringData.Length; j++)
                            {
                                if (stringData[j].Equals('"'))
                                {
                                    listIndex.Add(j);
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = listIndex.Count - 1; i >= 0; i--)
                {
                    if (isCenter)
                    {
                        stringData = stringData.Insert(listIndex[i], "line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';text-align: center;");
                    }
                    else
                    {
                        stringData = stringData.Insert(listIndex[i], "line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';");
                    }
                }

                if (stringData.Contains("<p>"))
                {
                    if (isCenter)
                    {
                        stringData = stringData.Replace("<p>", "<p style=\"line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';text-align: center;\">");
                    }
                    else
                    {
                        stringData = stringData.Replace("<p>", "<p style=\"line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';\">");
                    }
                }

                string ret = new Regex(@"line-height.+?;").Replace(stringData, "line-height: " + lineSpacing.ToString() + "%;");
                ret = new Regex(@"mso-margin-top-alt.+?;").Replace(ret, string.Empty);
                ret = new Regex(@"mso-margin-bottom-alt.+?;").Replace(ret, string.Empty);
                return ret;
            }
            else
                return string.Empty;
        }

        public static string NTSRemoveHtmlString(this string stringData)
        {
            return new Regex(@"<[^>]*>").Replace(stringData, string.Empty);
        }

        public static void NTSRemoveFirst(this WordDocument document, string given)
        {
            document.ReplaceFirst = true;
            var textSection = document.Find(given, false, true);
            WTextRange textRange = textSection.GetAsOneRange();
            //Get the owner textbody of the paragraph and remove the paragraph from it 
            WTextBody ownerTextBody = textRange.OwnerParagraph.OwnerTextBody;
            if (ownerTextBody != null)
                ownerTextBody.ChildEntities.Remove(textRange.OwnerParagraph);
        }

        public static void NTSReplaceHtml(this WordDocument document, string given, string html)
        {
            WordDocument replaceDoc = new WordDocument();
            IWSection htmlContent = replaceDoc.AddSection();
            if (htmlContent.Body.IsValidXHTML(html, XHTMLValidationType.Transitional))
            {
                htmlContent.Body.InsertXHTML(html);
            }
            document.Replace(given, replaceDoc, false, false);
        }

        public static void NTSReplaceImage(this WordDocument document, string given, string path, float? width = null, float? height = null, TextWrappingStyle textWrappingStyle = TextWrappingStyle.Inline)
        {

            WordDocument replate = new WordDocument();
            string filepath = path;
            FileStream imageStream = null;
            try
            {
                if (File.Exists(filepath))
                {
                    imageStream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite);
                    WParagraph paragraph = new WParagraph(document);
                    //Set the paragraph horizontal alignment 
                    paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                    //Append picture to the paragraph  
                    WPicture picture = (WPicture)paragraph.AppendPicture(imageStream);
                    if (width != null && height != null)
                    {
                        picture.Width = width.Value;
                        picture.Height = height.Value;
                    }
                    else
                    {
                        WSection section = document.Sections[0];
                        float clientWidth = section.PageSetup.ClientWidth;
                        float clientHeight = section.PageSetup.PageSize.Height - section.PageSetup.Margins.Top - section.PageSetup.Margins.Bottom;
                        float scalePer = 100;
                        if (picture.Width > clientWidth)
                        {
                            scalePer = clientWidth / picture.Width * 100;
                        }
                        else if (picture.Height > clientHeight)
                        {
                            scalePer = clientHeight / picture.Height * 100;
                        }
                        // This will resizes the width and height.
                        picture.WidthScale = scalePer;
                        picture.HeightScale = scalePer;
                    }

                    picture.TextWrappingStyle = textWrappingStyle;

                    TextBodyPart textBodyPart = new TextBodyPart(document);

                    //Add the paragraph to  
                    textBodyPart.BodyItems.Add(paragraph);

                    document.Replace(given, textBodyPart, false, true);
                    imageStream.Close();
                }
                else
                {
                    document.ReplaceFirst = true;
                    document.Replace(given, "", false, true);
                    document.ReplaceFirst = false;
                }
            }
            catch { imageStream?.Close(); }
        }


        public static void NTSAddText(this IWSection section, string text, float fontSize, bool bold, HorizontalAlignment align)
        {
            IWParagraph mPara = section.AddParagraph();
            mPara.ParagraphFormat.HorizontalAlignment = align;
            IWTextRange mtext = mPara.AppendText(text);
            mtext.CharacterFormat.FontSize = fontSize;
            mtext.CharacterFormat.Bold = bold;
            mtext.CharacterFormat.FontName = "Times New Roman";
        }

        public static void NTSReplaceFirst(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = true;
            document.Replace(given, !string.IsNullOrEmpty(replace) ? replace : string.Empty, false, true);
            document.ReplaceFirst = false;
        }

        public static void NTSReplaceAll(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = false;
            document.Replace(given, !string.IsNullOrEmpty(replace) ? replace : string.Empty, false, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document">Word</param>
        /// <param name="itemRepalte">Item replate</param>
        public static void ReplaceItem(this WordDocument document, ItemTextReplate itemRepalte)
        {
            if (document == null || itemRepalte == null)
                return;

            //Trường hợp repalteall == true thì ReplaceFirst false
            document.ReplaceFirst = itemRepalte.ReplateAll ? false : true;
            if (itemRepalte.Type == ReplateType.Html) // Replate html
            {
                WordDocument replaceDoc = new WordDocument();
                IWSection htmlContent = replaceDoc.AddSection();
                if (htmlContent.Body.IsValidXHTML(itemRepalte.Value, XHTMLValidationType.Transitional))
                {
                    htmlContent.Body.InsertXHTML(itemRepalte.Value);
                }
                document.Replace(itemRepalte.Key, replaceDoc, false, false);
            }
            else
            {
                document.Replace(itemRepalte.Key, !string.IsNullOrEmpty(itemRepalte.Value) ? itemRepalte.Value : string.Empty, false, true);
            }
        }

        public static void ReplaceImage(this WordDocument document, ItemImageReplate imageRepalte)
        {
            if (document == null || imageRepalte == null)
                return;
            WordDocument replate = new WordDocument();
            FileStream imageStream = null;
            try
            {
                if (File.Exists(imageRepalte.Path))
                {
                    imageStream = new FileStream(imageRepalte.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    WParagraph paragraph = new WParagraph(document);
                    //Set the paragraph horizontal alignment 
                    paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                    //Append picture to the paragraph  
                    WPicture picture = (WPicture)paragraph.AppendPicture(imageStream);
                    if (imageRepalte.Width != null && imageRepalte.Height != null)
                    {
                        picture.Width = imageRepalte.Width.Value;
                        picture.Height = imageRepalte.Height.Value;
                    }
                    else
                    {
                        WSection section = document.Sections[0];
                        float clientWidth = section.PageSetup.ClientWidth;
                        float clientHeight = section.PageSetup.PageSize.Height - section.PageSetup.Margins.Top - section.PageSetup.Margins.Bottom;
                        float scalePer = 100;
                        if (picture.Width > clientWidth)
                        {
                            scalePer = clientWidth / picture.Width * 100;
                        }
                        else if (picture.Height > clientHeight)
                        {
                            scalePer = clientHeight / picture.Height * 100;
                        }
                        // This will resizes the width and height.
                        picture.WidthScale = scalePer;
                        picture.HeightScale = scalePer;
                    }

                    picture.TextWrappingStyle = imageRepalte.WrappingStyle != null ? imageRepalte.WrappingStyle : TextWrappingStyle.Inline;

                    TextBodyPart textBodyPart = new TextBodyPart(document);

                    //Add the paragraph to  
                    textBodyPart.BodyItems.Add(paragraph);

                    //Trường hợp repalteall == true thì ReplaceFirst false
                    document.ReplaceFirst = imageRepalte.ReplateAll ? false : true;
                    document.Replace(imageRepalte.Key, textBodyPart, false, true);
                    imageStream.Close();
                }
                else
                {
                    document.ReplaceFirst = true;
                    document.Replace(imageRepalte.Key, "", false, true);
                    document.ReplaceFirst = false;
                }
            }
            catch { imageStream?.Close(); }
        }
    }
}
