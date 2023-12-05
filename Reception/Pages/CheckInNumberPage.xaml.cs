using Reception.Class;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для CheckInNumberPage.xaml
    /// </summary>
    public partial class CheckInNumberPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<FullRoom> fullRoom = new List<FullRoom>();
        List<ClassRoom> classRooms = new List<ClassRoom>();
        List<CheckIn> checkIns = new List<CheckIn>();

        FullRoom item = new FullRoom();
        private List<Visitor> visitors;
        int id;

        public CheckInNumberPage(int id,List<Visitor> visitors)
        {
            InitializeComponent();
            this.visitors = visitors;
            this.id = id;
        }

        public byte[] Foto(string str)
        {
            if (str != null)
            {
                string path = $@"{str.Trim()}";
                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }
                else
                    return null;
            }
            else return null;
        }

        public void LoadData()
        {
            ListRoom.ItemsSource = "";
            fullRoom.Clear();
            List<Room> rooms = db.Room.ToList();

            var room = from r in rooms
                       join cr in classRooms on r.ClassRoomID equals cr.ID
                       select new
                       {
                           ID = r.ID,
                           Photo = Foto(r.Photo),
                           NumberHuman = r.NumberHuman,
                           ClassRoomID = cr.Name,
                           Cost = r.Cost,
                       };

            foreach(var item in room)
            {
                fullRoom.Add(new FullRoom
                {
                    ID = item.ID,
                    Photo = item.Photo,
                    NumberHuman = item.NumberHuman,
                    ClassRoom = item.ClassRoomID,
                    Cost = Math.Round(item.Cost)
                });
            }
            ListRoom.ItemsSource = fullRoom;
        }

        public void datepicker(FullRoom item)
        {
            CheckInPicker.BlackoutDates.Clear();
            CheckOutPicker.BlackoutDates.Clear();
            checkIns = checkIns.Where(x => x.RoomID == item.ID).ToList();
            foreach (var i in checkIns)
            {
                if (i.DateCheckIn.Date >= DateTime.Now.Date)
                {
                    CheckInPicker.BlackoutDates.Add(new CalendarDateRange(i.DateCheckIn, i.DateCheckOut));
                    CheckOutPicker.BlackoutDates.Add(new CalendarDateRange(i.DateCheckIn, i.DateCheckOut));
                }
            }
        }

        public List<FullRoom> filterAll(string numPeople)
        {
            if (numPeople == "1 человек")
                return fullRoom = fullRoom.Where(x => x.NumberHuman == 1).ToList();
            else if (numPeople == "2 человека")
                return fullRoom = fullRoom.Where(x => x.NumberHuman == 2).ToList();
            else if (numPeople == "3 человека")
                return fullRoom = fullRoom.Where(x => x.NumberHuman == 3).ToList();
            else if (numPeople == "4 человека")
                return fullRoom = fullRoom.Where(x => x.NumberHuman == 4).ToList();
            else return fullRoom;
        }

        public void Filter()
        {
            LoadData();
            ListRoom.ItemsSource = "";
            string classR = "";
            string numPeople = "";

            if (ClassNumberBox.SelectedItem == null)
                classR = null;
            else
                classR = (ClassNumberBox.SelectedItem as ClassRoom).Name;

            if (NumberPeopleBox.SelectedItem == null)
                numPeople = null;
            else
                numPeople = ((ComboBoxItem)NumberPeopleBox.SelectedItem).Content.ToString();

            if (classR != null)
            {
                if (classR == "Все")
                {
                    fullRoom = filterAll(numPeople);
                }
                else
                {
                    if (numPeople == "1 человек")
                        fullRoom = fullRoom.Where(x => x.ClassRoom == classR && x.NumberHuman == 1).ToList();
                    else if (numPeople == "2 человека")
                        fullRoom = fullRoom.Where(x => x.ClassRoom == classR && x.NumberHuman == 2).ToList();
                    else if (numPeople == "3 человека")
                        fullRoom = fullRoom.Where(x => x.ClassRoom == classR && x.NumberHuman == 3).ToList();
                    else if (numPeople == "4 человека")
                        fullRoom = fullRoom.Where(x => x.ClassRoom == classR && x.NumberHuman == 4).ToList();
                }
                
                if (fullRoom.Count == 0)
                {
                    ListRoom.ItemsSource = "";
                }
                else if (numPeople == null)
                {
                    ListRoom.ItemsSource = "";
                }
                else
                    ListRoom.ItemsSource = fullRoom;
            }
            else
            {
                fullRoom = filterAll(numPeople);
                if (fullRoom.Count == 0)
                {
                    ListRoom.ItemsSource = "";
                }
                else
                    ListRoom.ItemsSource = fullRoom;
            }
                
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            classRooms = db.ClassRoom.ToList();
            ClassNumberBox.ItemsSource = classRooms;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInPicker.SelectedDate.HasValue == false || CheckOutPicker.SelectedDate.HasValue == false)
                MessageBox.Show("Вы не выбрали дату заселения/выселения");
            else if (CheckInPicker.SelectedDate > CheckOutPicker.SelectedDate)
                MessageBox.Show("Дата выселения не может быть раньше даты заселения");
            else
            {
                RoomForCheckIn roomForCheckIn = new RoomForCheckIn();
                roomForCheckIn.ID = item.ID;
                roomForCheckIn.Class = item.ClassRoom;
                roomForCheckIn.Num = item.NumberHuman;
                roomForCheckIn.DateCheckIn = (DateTime)CheckInPicker.SelectedDate;
                roomForCheckIn.DateCheckOut = (DateTime)CheckOutPicker.SelectedDate;
                NavigationService.Navigate(new CheckInServicePage(id, visitors, roomForCheckIn));
            }
        }

        private void BeforePageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ClassNumberBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void NumberPeopleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ListRoom.SelectedItem != null)
            {
                checkIns.Clear();
                checkIns = db.CheckIn.ToList();
                CheckInPicker.IsEnabled = true;
                CheckOutPicker.IsEnabled = true;
                item = ListRoom.SelectedItem as FullRoom;
                MessageBox.Show("Вы выбрали номер для проживания");
                datepicker(item);
            }
        }
    }
}
