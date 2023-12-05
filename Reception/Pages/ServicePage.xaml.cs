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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<Service> services = new List<Service>();

        public ServicePage()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            ListService.ItemsSource = "";
            services.Clear();

            services = db.Service.ToList();
            foreach(var i in services)
                i.Cost = Math.Round(i.Cost);
            ListService.ItemsSource = services;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                LoadData();
            else
            {
                services = services.Where(c => c.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
                ListService.ItemsSource = services;
            }
        }

        private void AddServiseButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDopServiceWindow takeDay = new TakeDopServiceWindow();

            var item = ListService.SelectedItem as Service;

            if (item != null)
            {
                List<CheckIn> checkIns = db.CheckIn.ToList();
                List<Room> rooms = db.Room.ToList();
                List<Int32> idroom = new List<Int32>();
                List<ListDopService> listDopServices = db.ListDopService.ToList();

                checkIns = checkIns.Where(x => x.StatusID == 1).ToList();

                takeDay.TakeDateForServicePanel.Visibility = Visibility.Collapsed;
                takeDay.NameServiceBlock.Text = item.Name;

                foreach (var i in rooms)
                {
                    foreach(var j in checkIns)
                    {
                        if (i.ID == j.RoomID)
                            idroom.Add(i.RoomNumber);
                    }
                }

                takeDay.IDRoomBox.ItemsSource = idroom;                

                if (takeDay.ShowDialog() == true)
                {
                    ListDopService listDop = new ListDopService();
                    if (takeDay.DayStartPicker.SelectedDate.HasValue == true && takeDay.DayOverPicker.SelectedDate.HasValue == true && takeDay.IDRoomBox.SelectedItem != null)
                    {
                        int textcombo = Convert.ToInt32(takeDay.IDRoomBox.SelectedItem);
                        listDop.DayStart = (DateTime)takeDay.DayStartPicker.SelectedDate;
                        listDop.DayOver = (DateTime)takeDay.DayOverPicker.SelectedDate;
                        rooms = rooms.Where(x => x.RoomNumber == textcombo).ToList();
                        foreach (var i in rooms)
                        {
                            foreach (var j in checkIns)
                            {
                                if (i.ID == j.RoomID)
                                    listDop.CheckInID = j.ID;
                            }
                        }
                        listDop.ServiceID = item.ID;
                    }
                    else
                    {
                        listDop.DayStart = Convert.ToDateTime("01-01-2000");
                        listDop.DayOver = Convert.ToDateTime("01-01-2000");
                    }

                    takeDay.Close();

                    if (listDop.DayStart > listDop.DayOver)
                    {
                        MessageBox.Show("Дата начала услуги не может быть позже даты окончания услуги");
                    }
                    else if (listDop.DayStart == Convert.ToDateTime("01-01-2000") || listDop.DayOver == Convert.ToDateTime("01-01-2000"))
                    {
                        MessageBox.Show("Дата начала или окончания услуги не может быть пуста");
                    }
                    else
                    {
                        db.ListDopService.Add(listDop);
                        db.SaveChanges();
                        MessageBox.Show("Услуга была добавлена");
                    }
                }
            }
            else
                MessageBox.Show("Чтобы добавить услугу, нажмите на элемент в списке");
        }
    }
}
