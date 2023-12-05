using Microsoft.Office.Interop.Word;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Reception.Class
{
    public class CreateSpravkaClass
    {
        ReseptionEntities db = new ReseptionEntities();

        public void CreateDoc(int id, int idCheck, List<ServiceFull> sf)
        {
            try
            {
                List<CheckIn> checkIns = db.CheckIn.Where(x => x.ID == idCheck).ToList();
                List<Worker> workers = db.Worker.Where(x => x.ID == id).ToList();
                List<Room> rooms = db.Room.ToList();
                List<ClassRoom> classRooms = db.ClassRoom.ToList();
                List<Visitor> visitors = db.Visitor.ToList();
                List<GroupVisitors> groupVisitors = db.GroupVisitors.ToList();
                foreach (var i in checkIns)
                    rooms = rooms.Where(x=>x.ID == i.RoomID).ToList();
                foreach (var i in rooms)
                    classRooms = classRooms.Where(x => x.ID == i.ClassRoomID).ToList();
                foreach(var i in groupVisitors)
                    foreach(var j in checkIns)
                    {
                        if (j.ID == i.CheckInID)
                            visitors = visitors.Where(x => x.ID == i.VisitorID).ToList();
                    }
                var days = 0.0;

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
                para1.Range.Text = "Справка";
                para1.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                para1.Range.Font.Bold = 1;
                para1.Range.Font.Name = "Times New Roman";
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.InsertParagraphAfter();

                para1.Range.Font.Bold = 0;
                para1.Range.Font.Size = 14;
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                foreach(var i in checkIns)
                {
                    foreach(var j in rooms)
                    {
                        foreach(var c in classRooms)
                        {
                            foreach(var v in visitors)
                            {
                                para1.Range.Text = $"Настоящим подтверждается, что {v.LastName} {v.FirstName} {v.Patronymic} проживал в гостинице \"Эдем\" в период с {i.DateCheckIn.ToString("dd.MM.yyyy")} по {i.DateCheckOut.ToString("dd.MM.yyyy")} в номере для {j.NumberHuman} человек(а), категории \"{c.Name}\".";
                                para1.Range.InsertParagraphAfter();
                            }
                        }
                    }
                }

                List<ListService> listServices = db.ListService.ToList();
                List<Service> services = db.Service.ToList();
                listServices = listServices.Where(x => x.CheckInID == idCheck).ToList();
                if (listServices.Count > 0)
                {
                    para1.Range.Text = "Услуги:";
                    para1.Range.InsertParagraphAfter();
                    for (int j = 0; j < listServices.Count; j++)
                    {
                        foreach (var c in services)
                        {
                            if (listServices[j].ServiceID == c.ID)
                            {
                                if (listServices[j].DayStart != listServices[j].DayOver)
                                {
                                    TimeSpan duration = listServices[j].DayOver - listServices[j].DayStart;
                                    days = duration.TotalDays + 1;
                                }
                                else days = 1;

                                para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн";
                                para1.Range.InsertParagraphAfter();
                            }
                        }
                    }
                }
                else
                {
                    para1.Range.Text = "Услуги отсутствуют";
                    para1.Range.InsertParagraphAfter();
                }

                List<ListDopService> listDopServices = db.ListDopService.ToList();
                listDopServices = listDopServices.Where(x => x.CheckInID == idCheck).ToList();
                if (listDopServices.Count > 0)
                {
                    para1.Range.Text = "Дополнительные услуги:";
                    para1.Range.InsertParagraphAfter();
                    for (int j = 0; j < listDopServices.Count; j++)
                    {
                        foreach (var c in sf)
                        {
                            if (listDopServices[j].ServiceID == c.ID)
                            {
                                if (listDopServices[j].DayStart != listDopServices[j].DayOver)
                                {
                                    TimeSpan duration = listDopServices[j].DayOver - listDopServices[j].DayStart;
                                    days = duration.TotalDays + 1;
                                }
                                else days = 1;

                                para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн";
                                para1.Range.InsertParagraphAfter();
                            }
                        }
                    }
                }
                else
                {
                    para1.Range.Text = "Дополнительные услуги отсутствуют";
                    para1.Range.InsertParagraphAfter();
                }

                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                para1.Range.Text = "Проведено: " + $"{DateTime.Now.ToString("dd.MM.yyyy")}";
                para1.Range.InsertParagraphAfter();

                para1.Range.Text = "Шишкин А.Б.";
                para1.Range.InsertParagraphAfter();
                para1.Range.Text = "Администратор";
                para1.Range.InsertParagraphAfter();
                para1.Range.Text = "Отдела по обслуживание гостей отеля";
                para1.Range.InsertParagraphAfter();

                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                para1.Range.Text = "Подпись _________________";
                para1.Range.InsertParagraphAfter();

                //Save the document
                object filename = @"D:\Курсовая 4 курс с Глебом\Справка на проживание.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Справка успешно создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
