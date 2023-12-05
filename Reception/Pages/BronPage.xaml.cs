using Reception.Class;
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
    /// Логика взаимодействия для BronPage.xaml
    /// </summary>
    public partial class BronPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<BronClass> bronClasses = new List<BronClass>();
        List<Room> rooms = new List<Room>();

        List<Visitor> visitors = new List<Visitor>();
        RoomForCheckIn roomFor = new RoomForCheckIn();
        List<ServiceFull> sf = new List<ServiceFull>();

        int id;

        public BronPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public void LoadData()
        {
            ListBron.ItemsSource = "";
            bronClasses.Clear();
            rooms.Clear();
            rooms = db.Room.ToList();
            List<ClassRoom> classRoom = db.ClassRoom.ToList();
            List<CheckIn> checkIns = db.CheckIn.Where(x => x.StatusID == 2).ToList();

            var bron = from c in checkIns
                       join r in rooms on c.RoomID equals r.ID
                       join cr in classRoom on r.ClassRoomID equals cr.ID
                       select new
                       {
                           ID = c.ID,
                           DayStart = c.DateCheckIn,
                           DayOver = c.DateCheckOut,
                           RoomID = r.ID,
                           NumberHuman = r.NumberHuman,
                           ClassRoom = cr.Name,
                           Sum = c.Sum,
                       };

            foreach (var item in bron)
            {
                bronClasses.Add(new BronClass
                {
                    ID = item.ID,
                    DayStart = item.DayStart,
                    DayOver = item.DayOver,
                    RoomID = item.RoomID,
                    NumberHuman = item.NumberHuman,
                    ClassRoom = item.ClassRoom,
                    Sum = Math.Round(item.Sum),
                });
            }
            ListBron.ItemsSource = bronClasses;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           LoadData();
        }

        private void AddBronButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ListBron.SelectedItem as BronClass;

            List<Service> services = new List<Service>();
            int idCheck = item.ID;

            if (item != null)
            {
                List<GroupVisitors> groupVisitors = db.GroupVisitors.ToList();
                List<ListService> listServices = db.ListService.Where(x=>x.CheckInID == item.ID).ToList();

                rooms = rooms.Where(x => x.ID == item.RoomID).ToList();

                foreach (var i in groupVisitors)
                    visitors = db.Visitor.Where(x => x.ID == i.VisitorID && i.CheckInID == item.ID).ToList();

                foreach (var j in rooms)
                {
                    roomFor.ID = j.ID;
                    roomFor.Class = item.ClassRoom;
                    roomFor.Num = item.NumberHuman;
                    roomFor.DateCheckOut = item.DayOver;
                    roomFor.DateCheckIn = item.DayStart;
                    break;
                }

                services = db.Service.ToList();
                foreach (var j in listServices)
                {
                    foreach (var i in services)
                    {
                        if (j.ServiceID == i.ID)
                        {
                            sf.Add(new ServiceFull
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Cost = Math.Round(i.Cost),
                                DayOver = j.DayOver,
                                DayStart = j.DayStart,
                            });
                            break;
                        }
                    }
                }

                NavigationService.Navigate(new BronFinishPage(id, visitors, roomFor, sf, idCheck));   
            }
            else
                MessageBox.Show("Чтобы перейти на оплату брони, нажмите на элемент в списке");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                LoadData();
            else
            {
                bronClasses = bronClasses.Where(c => c.ID == Convert.ToInt32(SearchBox.Text)).ToList();
                ListBron.ItemsSource = bronClasses;
            }
        }
    }
}
