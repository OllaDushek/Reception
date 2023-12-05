using Reception.Class;
using Reception.Dialogs;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reception.Pages
{
    /// <summary>
    /// Логика взаимодействия для CheckOutPage.xaml
    /// </summary>
    public partial class CheckOutPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<Service> services = new List<Service>();
        List<CheckIn> checkIns = new List<CheckIn>();
        List<ServiceFull> sf = new List<ServiceFull>();

        int IDroom;
        decimal sum = 0;
        int id;
        int idCheck;

        public CheckOutPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public void LoadData()
        {
            ListService.ItemsSource = "";
            IDRoomBox.IsEnabled = true;
            SumBox.Text = "";
            IDRoomBox.ItemsSource = "";
            checkIns.Clear();

            checkIns = db.CheckIn.ToList();
            List<Room> rooms = db.Room.ToList();
            List<Int32> idroom = new List<Int32>();

            checkIns = checkIns.Where(x => x.StatusID == 1).ToList();

            foreach (var i in rooms)
            {
                foreach (var j in checkIns)
                {
                    if (i.ID == j.RoomID)
                        idroom.Add(i.RoomNumber);
                }
            }

            IDRoomBox.ItemsSource = idroom;
        }

        public void ListInsertData(int idroom)
        {
            ListService.ItemsSource = "";
            services.Clear();
            sf.Clear();
            List<Room> rooms = db.Room.ToList();
            List<ListDopService> listDopServices = db.ListDopService.ToList();
            List<ServiceFull> ser = new List<ServiceFull>();
            List<ServiceFull> sernew = new List<ServiceFull>();

            services = db.Service.ToList();
            foreach(var i in rooms)
            {
                if(i.RoomNumber == idroom)
                    checkIns = checkIns.Where(x => x.RoomID == i.ID).ToList();
            }

            var s = from r in services
                    join l in listDopServices on r.ID equals l.ServiceID
                    select new
                    {
                        ID = l.ServiceID,
                        Name = r.Name,
                        Cost = Math.Round(r.Cost),
                        DayStart = l.DayStart,
                        DayOver = l.DayOver,
                    };

            foreach (var item in s)
            {
                ser.Add(new ServiceFull
                {
                    ID = item.ID,
                    Name = item.Name,
                    Cost = item.Cost,
                    DayStart = item.DayStart,
                    DayOver = item.DayOver,
                });
            }



            foreach (var i in listDopServices)
            {
                foreach (var j in checkIns)
                {
                    if(i.CheckInID == j.ID)
                    {
                        ser = ser.Where(x => x.ID == i.ServiceID && i.DayStart == x.DayStart && i.DayOver == x.DayOver).ToList();
                        foreach(var se in ser)
                        {
                            sernew.Add(new ServiceFull
                            {
                                ID = se.ID,
                                Name = se.Name,
                                Cost = se.Cost,
                                DayStart = se.DayStart,
                                DayOver = se.DayOver,
                            });
                        }
                        idCheck = j.ID;
                    }
                }
            }

            if (sernew.Count > 0)
            {
                foundnotfound.Text = "";
                ListService.Visibility = Visibility.Visible;
                ListService.ItemsSource = sernew;
                sf = sernew;
            }
            else
            {
                foundnotfound.Text = "отсутствует.";

                ListService.Visibility = Visibility.Collapsed;
            }
        }

        public void SumChange()
        {
            sum = 0;        
            if (sf.Count > 0)
            {
                foreach (var i in sf)
                {
                    if (i.DayStart != i.DayOver)
                    {
                        TimeSpan duration = (DateTime)i.DayOver - (DateTime)i.DayStart;
                        var days = duration.TotalDays + 1;
                        sum += Math.Round(i.Cost) * Convert.ToDecimal(days);
                    }
                    else sum += Math.Round(i.Cost);
                }
            }
            SumBox.Text = sum.ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void IDRoomBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IDRoomBox.SelectedItem != null)
                ListInsertData(Convert.ToInt32(IDRoomBox.SelectedItem));
            IDroom = Convert.ToInt32(IDRoomBox.SelectedItem);
            SumChange();
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (IDRoomBox.SelectedItem == null)
                MessageBox.Show("Вы не выбрали номер людей для выселения");
            else
            {
                if(sf.Count > 0)
                {
                    PaymentWindow payment = new PaymentWindow();

                    if (payment.ShowDialog() == true)
                    {
                        if (payment.CardBut.IsChecked == false && payment.RasrBut.IsChecked == false)
                            MessageBox.Show("Вы не выбрали способ оплаты");
                        else
                        {
                            foreach (var i in checkIns)
                            {
                                i.StatusID = 3;
                                i.Sum += sum;
                                if (payment.CardBut.IsChecked == true)
                                    i.PaymentID = 1;
                                else if (payment.RasrBut.IsChecked == true)
                                    i.PaymentID = 2;
                                db.SaveChanges();
                            }
                        }
                        payment.Close();

                        CheckOutButton.Visibility = Visibility.Collapsed;
                        CheckButton.Visibility = Visibility.Visible;
                        IDRoomBox.IsEnabled = false;
                        MessageBox.Show("Выселение прошло успешно");
                    }
                }
                else
                {
                    foreach (var i in checkIns)
                    {
                        i.StatusID = 3;
                        db.SaveChanges();
                    }
                    CheckOutButton.Visibility = Visibility.Collapsed;
                    CheckButton.Visibility = Visibility.Visible;
                    IDRoomBox.IsEnabled = false;
                    MessageBox.Show("Выселение прошло успешно");
                }
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            int i = IDroom;
            if(idCheck == 0)
            {
                foreach(var j in checkIns)
                {
                   idCheck = j.ID;
                }
            }
            LoadData();
            CheckButton.Visibility = Visibility.Collapsed;
            CheckOutButton.Visibility = Visibility.Visible;
            CreateCheckOutClass outClass = new CreateCheckOutClass();
            outClass.CreateDoc(id, idCheck, i, sf);
            CreateSpravkaClass spravkaClass = new CreateSpravkaClass();
            spravkaClass.CreateDoc(id, idCheck, sf);
        }
    }
}
