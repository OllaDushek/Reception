using Microsoft.Win32;
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
    /// Логика взаимодействия для AddRoomPage.xaml
    /// </summary>
    public partial class AddRoomPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();
        System.Windows.Controls.Image photoimage = new System.Windows.Controls.Image();

        int id;
        string photopath;

        public AddRoomPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public int CatchError()
        {
            List<Room> rooms = db.Room.ToList();
            bool a = true;
                if (String.IsNullOrWhiteSpace(NumberRoomBox.Text) || String.IsNullOrWhiteSpace(NumberHumanBox.Text)
                    || String.IsNullOrWhiteSpace(CostBox.Text) || ClassRoomBox.SelectedItem == null || ImageRoom.Children == null)
                    return 1;
                else if (!CostBox.Text.All(char.IsDigit))
                    return 2;
                else if (Convert.ToInt32(NumberHumanBox.Text) > 4 || Convert.ToInt32(NumberHumanBox.Text) < 1)
                    return 4;
                else
                {
                    foreach (var i in rooms)
                    {
                        if (NumberRoomBox.Text == i.RoomNumber.ToString())
                            return 3;
                        else a = false;
                    }
                    return 0;
                }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<ClassRoom> classRooms = db.ClassRoom.ToList();
            switch (CatchError())
            {
                case 1:
                    MessageBox.Show("Вы не заполнили все поля");
                    break;
                case 2:
                    MessageBox.Show("Вы неправильно заполнили поле цены номера");
                    break;
                case 3:
                    MessageBox.Show("Вы не можете дать комнате номер, который уже присвоен другой комнате");
                    break;
                case 4:
                    MessageBox.Show("Вы не можете присвоить номеру количество людей больше 4 или меньше 1");
                    break;
                case 0:
                    Room room = new Room();
                    room.Photo = photopath;
                    room.NumberHuman = Convert.ToInt32(NumberHumanBox.Text);
                    room.RoomNumber = Convert.ToInt32(NumberRoomBox.Text);
                    room.Cost = Convert.ToDecimal(CostBox.Text);
                    foreach (var i in classRooms)
                    {
                        if ((ClassRoomBox.SelectedItem as ClassRoom).Name == i.Name)
                            room.ClassRoomID = i.ID;
                    }
                    db.Room.Add(room);
                    db.SaveChanges();
                    MessageBox.Show("Данные были добавлены");
                    NavigationService.Navigate(new RoomForWorkerPage(id));
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassRoomBox.ItemsSource = db.ClassRoom.Where(x=>x.ID != 1).ToList();
        }

        private void ImageRoom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageRoom.Children.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "image files (*.png)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog().Value)
            {
                string file = openFileDialog.FileName;
                photoimage.Width = 170;
                photoimage.Height = 100;
                photoimage.Source = new BitmapImage(new Uri(file));
                ImageRoom.Children.Add(photoimage);
                photopath = file;
            }
        }
    }
}
