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
    public class CreateDocAllClass
    {

        ReseptionEntities db = new ReseptionEntities();

        public void CreateDoc(int id, DateTime dayStart, DateTime dayOver)
        {
            try
            {
                List<CheckIn> checkIn = db.CheckIn.Where(x => x.StatusID == 1 && x.DateCheckIn >= dayStart && x.DateCheckOut <= dayOver).ToList();
                List<CheckIn> checkOut = db.CheckIn.Where(x => x.StatusID == 3 && x.DateCheckIn >= dayStart && x.DateCheckOut <= dayOver).ToList();
                List<CheckIn> checkBron = db.CheckIn.Where(x => x.StatusID == 2 && x.DateCheckIn >= dayStart && x.DateCheckOut <= dayOver).ToList();
                List<Worker> workers = db.Worker.Where(x => x.ID == id).ToList();
                List<Room> rooms = db.Room.ToList();
                List<ClassRoom> classRooms = db.ClassRoom.ToList();
                List<Service> services = db.Service.ToList();
                List<Visitor> visitors = db.Visitor.ToList();
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
                para1.Range.Text = "Отчет";
                para1.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                para1.Range.Font.Bold = 1;
                para1.Range.Font.Name = "Times New Roman";
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.InsertParagraphAfter();

                para1.Range.Font.Bold = 0;
                para1.Range.Font.Size = 14;
                para1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                para1.Range.Text = "Дата начала: " + $"{dayStart.ToString("dd.MM.yyyy")}";
                para1.Range.InsertParagraphAfter();
                para1.Range.Text = "Дата окончания: " + $"{dayOver.ToString("dd.MM.yyyy")}";
                para1.Range.InsertParagraphAfter();

                foreach (var i in workers)
                    para1.Range.Text = "Создал: " + $"{i.LastName} {i.FirstName} {i.Patronymic}";
                para1.Range.InsertParagraphAfter();

                //checkIn
                if (checkIn.Count == 0)
                {
                    para1.Range.Text = "Всего было проведено 0 заселений";
                    para1.Range.InsertParagraphAfter();
                }
                else
                {
                    para1.Range.Text = $"Всего было проведено {checkIn.Count} заселений(ие):";
                    para1.Range.InsertParagraphAfter();
                    for (int i = 0; i < checkIn.Count; i++)
                    {
                        para1.Range.Text = $"{i+1}. Чек №{checkIn[i].ID}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата заселения: {checkIn[i].DateCheckIn.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата выселения: {checkIn[i].DateCheckOut.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        foreach(var j in rooms)
                        {
                            foreach(var c in classRooms)
                            {
                                if(j.ID == checkIn[i].RoomID && j.ClassRoomID == c.ID)
                                {
                                    para1.Range.Text = $"Класс номера: {c.Name}, стоимость: {Math.Round(j.Cost)}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        if (checkIn[i].BusinessTrip == true)
                            para1.Range.Text = $"Командировка: есть";
                        else para1.Range.Text = $"Командировка: нету";
                        para1.Range.InsertParagraphAfter();

                        if (checkIn[i].PaymentID == 1)
                            para1.Range.Text = "Расчет картой";
                        else para1.Range.Text = "Расчет наличными";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = "Жильцы:";
                        para1.Range.InsertParagraphAfter();

                        List<GroupVisitors> groupVisitors = db.GroupVisitors.ToList();
                        groupVisitors = groupVisitors.Where(x=>x.CheckInID == checkIn[i].ID).ToList();
                        for (int j = 0; j < groupVisitors.Count; j++)
                        {
                            foreach (var c in visitors)
                            {
                                if (groupVisitors[j].VisitorID == c.ID)
                                {
                                    para1.Range.Text = $"{j + 1}. {c.LastName} {c.FirstName} {c.Patronymic}, телефон: {c.Phone}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        List<ListService> listServices = db.ListService.ToList();
                        listServices = listServices.Where(x=>x.CheckInID == checkIn[i].ID).ToList();
                        if(listServices.Count > 0)
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

                                        para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн Х {Math.Round(c.Cost)}p";
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
                        listDopServices = listDopServices.Where(x => x.CheckInID == checkIn[i].ID).ToList();
                        if (listDopServices.Count > 0)
                        {
                            para1.Range.Text = "Дополнительные услуги:";
                            para1.Range.InsertParagraphAfter();

                            for (int j = 0; j < listDopServices.Count; j++)
                            {
                                foreach (var c in services)
                                {
                                    if (listDopServices[j].ServiceID == c.ID)
                                    {
                                        if (listDopServices[j].DayStart != listDopServices[j].DayOver)
                                        {
                                            TimeSpan duration = listDopServices[j].DayOver - listDopServices[j].DayStart;
                                            days = duration.TotalDays + 1;
                                        }
                                        else days = 1;

                                        para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн Х {Math.Round(c.Cost)}p";
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

                        para1.Range.Text = $"Сумма: {Math.Round(checkIn[i].Sum)}";
                        para1.Range.InsertParagraphAfter();
                    }
                }

                //checkBron
                if (checkBron.Count == 0)
                {
                    para1.Range.Text = "Всего было проведено 0 бронирований";
                    para1.Range.InsertParagraphAfter();
                }
                else
                {
                    para1.Range.Text = $"Всего было проведено {checkBron.Count} бронирований(ие):";
                    para1.Range.InsertParagraphAfter();
                    for (int i = 0; i < checkBron.Count; i++)
                    {
                        para1.Range.Text = $"{i + 1}. Чек №{checkBron[i].ID}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата заселения: {checkBron[i].DateCheckIn.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата выселения: {checkBron[i].DateCheckOut.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        foreach (var j in rooms)
                        {
                            foreach (var c in classRooms)
                            {
                                if (j.ID == checkBron[i].RoomID && j.ClassRoomID == c.ID)
                                {
                                    para1.Range.Text = $"Класс номера: {c.Name}, стоимость: {Math.Round(j.Cost)}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        if (checkBron[i].BusinessTrip == true)
                            para1.Range.Text = $"Командировка: есть";
                        else para1.Range.Text = $"Командировка: нету";
                        para1.Range.InsertParagraphAfter();

                        if (checkBron[i].PaymentID == 1)
                            para1.Range.Text = "Расчет картой";
                        else para1.Range.Text = "Расчет наличными";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = "Жильцы:";
                        para1.Range.InsertParagraphAfter();

                        List<GroupVisitors> groupVisitors = db.GroupVisitors.ToList();
                        groupVisitors = groupVisitors.Where(x => x.CheckInID == checkBron[i].ID).ToList();
                        for (int j = 0; j < groupVisitors.Count; j++)
                        {
                            foreach (var c in visitors)
                            {
                                if (groupVisitors[j].VisitorID == c.ID)
                                {
                                    para1.Range.Text = $"{j + 1}. {c.LastName} {c.FirstName} {c.Patronymic}, телефон: {c.Phone}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        List<ListService> listServices = db.ListService.ToList();
                        listServices = listServices.Where(x => x.CheckInID == checkBron[i].ID).ToList();
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

                                        para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн Х {Math.Round(c.Cost)}p";
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

                        para1.Range.Text = $"Сумма: {Math.Round(checkBron[i].Sum)}";
                        para1.Range.InsertParagraphAfter();
                    }
                }

                //checkOut
                if (checkOut.Count == 0)
                {
                    para1.Range.Text = "Всего было проведено 0 выселений";
                    para1.Range.InsertParagraphAfter();
                }
                else
                {
                    para1.Range.Text = $"Всего было проведено {checkOut.Count} выселений(ие):";
                    para1.Range.InsertParagraphAfter();
                    for (int i = 0; i < checkOut.Count; i++)
                    {
                        para1.Range.Text = $"{i + 1}. Чек №{checkOut[i].ID}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата заселения: {checkOut[i].DateCheckIn.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = $"Дата выселения: {checkOut[i].DateCheckOut.ToString("dd.MM.yyyy")}";
                        para1.Range.InsertParagraphAfter();

                        foreach (var j in rooms)
                        {
                            foreach (var c in classRooms)
                            {
                                if (j.ID == checkOut[i].RoomID && j.ClassRoomID == c.ID)
                                {
                                    para1.Range.Text = $"Класс номера: {c.Name}, стоимость: {Math.Round(j.Cost)}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        if (checkOut[i].BusinessTrip == true)
                            para1.Range.Text = $"Командировка: есть";
                        else para1.Range.Text = $"Командировка: нету";
                        para1.Range.InsertParagraphAfter();

                        if (checkOut[i].PaymentID == 1)
                            para1.Range.Text = "Расчет картой";
                        else para1.Range.Text = "Расчет наличными";
                        para1.Range.InsertParagraphAfter();

                        para1.Range.Text = "Жильцы:";
                        para1.Range.InsertParagraphAfter();

                        List<GroupVisitors> groupVisitors = db.GroupVisitors.ToList();
                        groupVisitors = groupVisitors.Where(x => x.CheckInID == checkOut[i].ID).ToList();
                        for (int j = 0; j < groupVisitors.Count; j++)
                        {
                            foreach (var c in visitors)
                            {
                                if (groupVisitors[j].VisitorID == c.ID)
                                {
                                    para1.Range.Text = $"{j + 1}. {c.LastName} {c.FirstName} {c.Patronymic}, телефон: {c.Phone}";
                                    para1.Range.InsertParagraphAfter();
                                }
                            }
                        }

                        List<ListService> listServices = db.ListService.ToList();
                        listServices = listServices.Where(x => x.CheckInID == checkOut[i].ID).ToList();
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

                                        para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн Х {Math.Round(c.Cost)}p";
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
                        listDopServices = listDopServices.Where(x => x.CheckInID == checkOut[i].ID).ToList();
                        if (listDopServices.Count > 0)
                        {
                            para1.Range.Text = "Дополнительные услуги:";
                            para1.Range.InsertParagraphAfter();

                            for (int j = 0; j < listDopServices.Count; j++)
                            {
                                foreach (var c in services)
                                {
                                    if (listDopServices[j].ServiceID == c.ID)
                                    {
                                        if (listDopServices[j].DayStart != listDopServices[j].DayOver)
                                        {
                                            TimeSpan duration = listDopServices[j].DayOver - listDopServices[j].DayStart;
                                            days = duration.TotalDays + 1;
                                        }
                                        else days = 1;

                                        para1.Range.Text = $"{j + 1}. {c.Name}, {days}дн Х {Math.Round(c.Cost)}p";
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

                        para1.Range.Text = $"Сумма: {Math.Round(checkOut[i].Sum)}";
                        para1.Range.InsertParagraphAfter();
                    }
                }

                //Add paragraph with Heading 1 style
                Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading2 = "Заголовок 1";
                para2.Range.set_Style(ref styleHeading2);
                decimal Summa = 0;
                foreach (var i in checkIn)
                    Summa += i.Sum;
                foreach (var i in checkOut)
                    Summa += i.Sum;
                para2.Range.Text = $"Итого: {Math.Round(Summa)}р";
                para2.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                para2.Range.Font.Bold = 1;
                para2.Range.Font.Name = "Times New Roman";
                para2.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                para2.Range.InsertParagraphAfter();

                para2.Range.Font.Bold = 0;
                para2.Range.Font.Size = 14;
                para2.Range.InsertParagraphAfter();

                para2.Range.Text = "Проведено: " + $"{DateTime.Now.ToString("dd.MM.yyyy hh:mm")}";
                para2.Range.InsertParagraphAfter();

                //Save the document
                object filename = @"D:\Курсовая 4 курс с Глебом\CheckAll.docx";
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