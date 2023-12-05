using Reception.Class;
using Reception.Dialogs;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для BronFinishPage.xaml
    /// </summary>
    public partial class BronFinishPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        private List<Visitor> visitors;
        private List<ServiceFull> sf;
        private RoomForCheckIn roomFor;

        decimal sum = 0;
        int id;
        int idCheck;

        public BronFinishPage(int id, List<Visitor> visitors, RoomForCheckIn roomFor, List<ServiceFull> sf, int idCheck)
        {
            InitializeComponent();
            this.visitors = visitors;
            this.roomFor = roomFor;
            this.sf = sf;
            this.id = id;
            this.idCheck = idCheck;
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

        public void LoadData()
        {
            ListService.ItemsSource = "";
            CountVisitorBox.Text = visitors.Count.ToString();
            ClassRoomBox.Text = roomFor.Class;
            List<CheckIn> cI = db.CheckIn.Where(x => x.ID == idCheck).ToList();
            foreach(var i in cI)
            {
                if (i.BusinessTrip == true)
                    CheckComandirBox.IsChecked = true;
                else CheckComandirBox.IsChecked = false;
            }
            CountVisitorRoomBox.Text = roomFor.Num.ToString();
            DateStartBox.Text = roomFor.DateCheckIn.Date.ToString("dd.MM.yyyy");
            DateOverBox.Text = roomFor.DateCheckOut.Date.ToString("dd.MM.yyyy");
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

        private void AddCheckInButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentWindow payment = new PaymentWindow();

            if (payment.ShowDialog() == true)
            {
                if (payment.CardBut.IsChecked == false && payment.RasrBut.IsChecked == false)
                    MessageBox.Show("Вы не выбрали способ оплаты");
                else
                {
                    var uRow = db.CheckIn.Where(w => w.ID == idCheck).FirstOrDefault();
                    uRow.StatusID = 1;
                    if (payment.CardBut.IsChecked == true)
                        uRow.PaymentID = 1;
                    else if (payment.RasrBut.IsChecked == true)
                        uRow.PaymentID = 2;
                    db.SaveChanges();

                }
                payment.Close();

                AddCheckInButton.Visibility = Visibility.Collapsed;
                ListService.IsEnabled = false;
                CheckButton.Visibility = Visibility.Visible;
            }           
        }

        private void BeforePageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            //create check v vide PDF file
            CreateCheckInClass create = new CreateCheckInClass();
            create.CreateDoc(id, idCheck, roomFor, sf);

            NavigationService.Navigate(new CheckInPage(id));
        }
    }
}
