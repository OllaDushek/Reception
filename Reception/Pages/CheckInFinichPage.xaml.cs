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
    /// Логика взаимодействия для CheckInFinichPage.xaml
    /// </summary>
    public partial class CheckInFinichPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        GroupVisitors groupVisitors = new GroupVisitors();
        ListService serviceList = new ListService();
        CheckIn checkIn = new CheckIn();

        private List<Visitor> visitors;
        private List<ServiceFull> sf;
        private RoomForCheckIn roomFor;

        decimal sum = 0;
        int id;

        public CheckInFinichPage(int id, List<Visitor> visitors, RoomForCheckIn roomFor, List<ServiceFull> sf)
        {
            InitializeComponent();
            this.visitors = visitors;
            this.roomFor = roomFor;
            this.sf = sf;
            this.id = id;
        }

        public void SumChange()
        {
            List<Room> rooms = new List<Room>();
            sum = 0;
            rooms = db.Room.ToList();
            foreach (var i in rooms)
            {
                if (i.ID == roomFor.ID)
                {
                    if (roomFor.DateCheckIn != roomFor.DateCheckOut)
                    {
                        TimeSpan duration = roomFor.DateCheckOut - roomFor.DateCheckIn;
                        var days = duration.TotalDays + 1;
                        sum += Math.Round(i.Cost) * Convert.ToDecimal(days);
                    }
                    else sum += Math.Round(i.Cost);
                }
            }
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

        public void VisibilityButton()
        {
            AddCheckInButton.Visibility = Visibility.Visible;
            if (sf.Count > 0)
            {
                foundnotfound.Text = "";
                ListService.Visibility = Visibility.Visible;
                ListService.ItemsSource = sf;
            }
            else
            {
                foundnotfound.Text = "отсутствует.";
                ListService.Visibility = Visibility.Collapsed;
            }
        }

        public void LoadData()
        {
            ListService.ItemsSource = "";
            CountVisitorBox.Text = visitors.Count.ToString();
            ClassRoomBox.Text = roomFor.Class;
            CountVisitorRoomBox.Text = roomFor.Num.ToString();
            DateStartBox.Text = roomFor.DateCheckIn.Date.ToString("dd.MM.yyyy");
            DateOverBox.Text = roomFor.DateCheckOut.Date.ToString("dd.MM.yyyy");
            List<CheckIn> check = db.CheckIn.ToList();
            if(check.Count > 0)
            {
                foreach (var i in check)
                {
                    if (i.RoomID == roomFor.ID && i.StatusID == 1)
                        AddCheckInButton.Visibility = Visibility.Collapsed;
                    else
                        VisibilityButton();
                }
            }
            else
                VisibilityButton();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            SumChange();
        }

        private void DeleteServiseButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ListService.SelectedItem as ServiceFull;
            if (item != null)
            {
                sf.Remove(item);
                LoadData();
                MessageBox.Show("Услуга удалена");
                SumChange();
            }
            else
                MessageBox.Show("Чтобы удалить услугу, нажмите на элемент в списке");
        }

        public void addBron()
        {
            List<CheckIn> checkIns = db.CheckIn.ToList();
            int idCheck;
            if (checkIns.Count > 0)
                idCheck = checkIns.Last().ID;
            else
                idCheck = checkIns.FirstOrDefault().ID;

                foreach (var i in visitors)
                {
                    groupVisitors.VisitorID = i.ID;
                    groupVisitors.CheckInID = idCheck;
                    db.GroupVisitors.Add(groupVisitors);
                    db.SaveChanges();
                }

            foreach (var i in sf)
            {
                serviceList.ServiceID = i.ID;
                serviceList.CheckInID = idCheck;
                serviceList.DayStart = i.DayStart.GetValueOrDefault();
                serviceList.DayOver = i.DayOver.GetValueOrDefault();
                db.ListService.Add(serviceList);
                db.SaveChanges();
            }
        }

        private void AddCheckInButton_Click(object sender, RoutedEventArgs e)
        {
            List<Status> statuses = db.Status.ToList();
            PaymentWindow payment = new PaymentWindow();

            if (payment.ShowDialog() == true)
            {
                if (payment.CardBut.IsChecked == false && payment.RasrBut.IsChecked == false)
                    MessageBox.Show("Вы не выбрали способ оплаты");
                else
                {
                    foreach (var i in statuses)
                    {
                        if (i.Name == "Зарезервирован")
                        {
                            checkIn.DateCheckIn = roomFor.DateCheckIn;
                            checkIn.DateCheckOut = roomFor.DateCheckOut;
                            checkIn.RoomID = roomFor.ID;
                            if (CheckComandirBox.IsChecked == true)
                                checkIn.BusinessTrip = true;
                            else
                                checkIn.BusinessTrip = false;
                            checkIn.Sum = Convert.ToInt32(SumBox.Text);
                            checkIn.StatusID = i.ID;
                            checkIn.WorkerID = id;
                            if (payment.CardBut.IsChecked == true)
                                checkIn.PaymentID = 1;
                            else if (payment.RasrBut.IsChecked == true)
                                checkIn.PaymentID = 2;
                            db.CheckIn.Add(checkIn);
                            db.SaveChanges();
                        }
                    }
                }
                payment.Close();

                addBron();
                ButtonsInBron.Visibility = Visibility.Collapsed;
                ListService.IsEnabled = false;
                CheckButton.Visibility = Visibility.Visible;
            }           
        }

        private void AddBronButton_Click(object sender, RoutedEventArgs e)
        {
            List<Status> statuses = db.Status.ToList();

            foreach (var i in statuses)
            {
                if (i.Name == "Забронирован")
                {
                    checkIn.DateCheckIn = roomFor.DateCheckIn;
                    checkIn.DateCheckOut = roomFor.DateCheckOut;
                    checkIn.RoomID = roomFor.ID;
                    if (CheckComandirBox.IsChecked == true)
                        checkIn.BusinessTrip = true;
                    else
                        checkIn.BusinessTrip = false;
                    checkIn.Sum = Convert.ToInt32(SumBox.Text);
                    checkIn.StatusID = i.ID;
                    checkIn.WorkerID = id;
                    db.CheckIn.Add(checkIn);
                    db.SaveChanges();
                }
            }
            addBron();
            MessageBox.Show("Номер забронирован");
            NavigationService.Navigate(new CheckInPage(id));
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            //create check v vide PDF file
            CreateCheckInClass create = new CreateCheckInClass();
            List<CheckIn> checkIns = db.CheckIn.ToList();
            int idCheck = 0;
            if (checkIns.Count == 1)
                idCheck = checkIns.First().ID;
            else if (checkIns.Count > 1)
                idCheck = checkIns.Last().ID;
            create.CreateDoc(id, idCheck, roomFor, sf);

            NavigationService.Navigate(new CheckInPage(id));
        }
    }
}
