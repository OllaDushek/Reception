using Microsoft.Office.Interop.Word;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reception.Class
{
    public class CreateCheckOutClass
    {
        ReseptionEntities db = new ReseptionEntities();

        public void CreateDoc(int idworker, int idCheck, int IDroom, List<ServiceFull> sf)
        {
            try
            {
                List<CheckIn> checkIns = db.CheckIn.Where(x => x.ID == idCheck).ToList();
                List<Worker> workers = db.Worker.Where(x => x.ID == idworker).ToList();
                var day = 0.0;
                decimal Sum = 0;

                //Create an instance for word app
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.
                winword.Visible = false;

                //Create a missing variable for missing value
                object missing = System.Reflection.Missing.Value;

                //Create a new document
                Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add paragraph with Heading 1 style
                Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading1 = "Заголовок 1";
                para1.Range.set_Style(ref styleHeading1);
                para1.Range.Text = "Выселение";
                para1.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                para1.Range.Font.Bold = 1;
                para1.Range.Font.Name = "Times New Roman";
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.InsertParagraphAfter();

                para1.Range.Font.Bold = 0;
                para1.Range.Font.Size = 14;
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                para1.Range.Text = "Чек №" + $"{idCheck}";
                para1.Range.InsertParagraphAfter();

                foreach (var i in checkIns)
                {
                    if (i.PaymentID == 1)
                        para1.Range.Text = "Расчет картой";
                    else para1.Range.Text = "Расчет наличными";
                }
                para1.Range.InsertParagraphAfter();

                para1.Range.Text = "Дата выдачи: " + $"{DateTime.Now.ToString("dd.MM.yyyy hh:mm")}";
                para1.Range.InsertParagraphAfter();

                foreach (var i in workers)
                    para1.Range.Text = "Выдан: " + $"{i.LastName} {i.FirstName} {i.Patronymic}";
                para1.Range.InsertParagraphAfter();

                para1.Range.Text = $"Проживание в номере: {IDroom}";
                para1.Range.InsertParagraphAfter();

                if (sf.Count == 0)
                {
                    para1.Range.Text = "Доп. услуги: отсутствуют";
                    para1.Range.InsertParagraphAfter();
                }
                else
                {
                    para1.Range.Text = "Доп. услуги: ";
                    para1.Range.InsertParagraphAfter();

                    for (int i = 0; i < sf.Count; i++)
                    {
                        if (sf[i].DayStart != sf[i].DayOver)
                        {
                            TimeSpan duration = (DateTime)sf[i].DayOver - (DateTime)sf[i].DayStart;
                            day = duration.TotalDays + 1;
                            Sum += Math.Round(sf[i].Cost) * Convert.ToDecimal(day);
                        }
                        else
                        {
                            day = 1;
                            Sum += Math.Round(sf[i].Cost);
                        }
                        para1.Range.Text = $"{i + 1}." + $" {sf[i].Name}: {day} дн X {Math.Round(sf[i].Cost)}р";
                        para1.Range.InsertParagraphAfter();
                    }
                }

                //Add paragraph with Heading 1 style
                Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading2 = "Заголовок 1";
                para2.Range.set_Style(ref styleHeading2);
                foreach (var i in checkIns)
                    para2.Range.Text = $"Итого: {Sum}р";
                para2.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                para2.Range.Font.Bold = 1;
                para2.Range.Font.Name = "Times New Roman";
                para2.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                para2.Range.InsertParagraphAfter();

                para2.Range.Font.Bold = 0;
                para2.Range.Font.Size = 14;
                para2.Range.InsertParagraphAfter();

                foreach (var i in checkIns)
                {
                    para2.Range.Text = "Дата заселения: " + $"{i.DateCheckIn.ToString("dd.MM.yyyy")}";
                    para2.Range.InsertParagraphAfter();
                    para2.Range.Text = "Дата выселения: " + $"{i.DateCheckOut.ToString("dd.MM.yyyy")}";
                    para2.Range.InsertParagraphAfter();
                }

                //Save the document
                object filename = @"D:\Курсовая 4 курс с Глебом\checkOutReception.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Чек успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
