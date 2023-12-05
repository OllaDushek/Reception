using Reception.Class;
using Reception.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для RoomForWorkerPage.xaml
    /// </summary>
    public partial class RoomForWorkerPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        int id;

        public RoomForWorkerPage(int id)
        {
            InitializeComponent();
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
            List<FullRoom> roomList = new List<FullRoom>();
            FullRoom fullRoom = new FullRoom();
            List<Room> roomCheck = db.Room.ToList();
            List<Room> rooms = db.Room.ToList();
            List<ClassRoom> classRooms = db.ClassRoom.ToList();
            List<CheckIn> checkIns = db.CheckIn.Where(x=>x.StatusID != 3).ToList();
            List<Room> roomsNew = new List<Room>();

            foreach(var i in rooms)
            {
                foreach (var r in checkIns)
                {
                    if (r.RoomID == i.ID)
                    {
                        roomsNew.Add(i);
                    }
                }
            }

            foreach (var i in roomsNew)
                rooms.Remove(i);

            foreach (var i in roomCheck)
            {
                foreach(var j in classRooms)
                {
                    foreach(var c in checkIns)
                    {
                        if (i.ID == c.RoomID && i.ClassRoomID == j.ID)
                        {
                            fullRoom.ID = i.ID;
                            fullRoom.Photo = Foto(i.Photo);
                            fullRoom.NumberHuman = i.NumberHuman;
                            fullRoom.Cost = i.Cost;
                            fullRoom.ClassRoom = j.Name;
                            if (c.StatusID == 1) fullRoom.StatusRoom = "Зарезервирован";
                            else if (c.StatusID == 2) fullRoom.StatusRoom = "Забронирован";
                            roomList.Add(new FullRoom{
                                ID = fullRoom.ID,
                                Photo = fullRoom.Photo,
                                NumberHuman = fullRoom.NumberHuman,
                                Cost = Math.Round(fullRoom.Cost),
                                ClassRoom = fullRoom.ClassRoom,
                                StatusRoom = fullRoom.StatusRoom,
                            });
                        }
                    }
                }
            }

            foreach (var i in rooms)
            {
                foreach (var j in classRooms)
                {
                    if (i.ClassRoomID == j.ID)
                    {
                        fullRoom.ID = i.ID;
                        fullRoom.Photo = Foto(i.Photo);
                        fullRoom.NumberHuman = i.NumberHuman;
                        fullRoom.Cost = i.Cost;
                        fullRoom.ClassRoom = j.Name;
                        fullRoom.StatusRoom = "Свободен";
                        roomList.Add(new FullRoom
                        {
                            ID = fullRoom.ID,
                            Photo = fullRoom.Photo,
                            NumberHuman = fullRoom.NumberHuman,
                            Cost = Math.Round(fullRoom.Cost),
                            ClassRoom = fullRoom.ClassRoom,
                            StatusRoom = fullRoom.StatusRoom,
                        });
                    }
                }
            }

            ListRooms.ItemsSource = roomList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ListRooms.SelectedItem as FullRoom;
            if (item != null)
            {
                if (item.StatusRoom != "Свободен")
                    MessageBox.Show("Вы не можете изменить номер, который уже зарезервирован или забронирован");
                else
                    NavigationService.Navigate(new UpdateRoomPage(item, id));                
            }
            else
                MessageBox.Show("Чтобы изменить номер, нажмите на элемент в списке");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотете удалить - этот номер?",
                            "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var item = ListRooms.SelectedItem as FullRoom;
                    if (item != null)
                    {
                        if (item.StatusRoom != "Свободен")
                            MessageBox.Show("Вы не можете удалить номер, который уже зарезервирован или забронирован");
                        else
                        {
                            var uRow = db.Room.Where(w => w.ID == item.ID).FirstOrDefault();
                            db.Room.Remove(uRow);
                            db.SaveChanges();
                            LoadData();
                            MessageBox.Show("Номер был удален");
                        }
                    }
                    else
                        MessageBox.Show("Чтобы удалить номер, нажмите на элемент в списке");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRoomPage(id));
        }
    }
}
