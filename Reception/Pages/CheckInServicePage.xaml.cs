using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Reception.Class;
using Reception.Dialogs;
using Reception.Model;

namespace Reception.Pages
{
    /// <summary>
    /// Логика взаимодействия для CheckInServicePage.xaml
    /// </summary>
    public partial class CheckInServicePage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<ServiceFull> serviceFulls = new List<ServiceFull>();
        List<ServiceFull> sf = new List<ServiceFull>();

        private List<Visitor> visitors;
        private RoomForCheckIn roomFor;
        int id;

        public CheckInServicePage(int id, List<Visitor> visitors, RoomForCheckIn roomFor)
        {
            InitializeComponent();
            this.visitors = visitors;
            this.roomFor = roomFor;
            this.id = id;
        }

        public void LoadData()
        {
            ListService.ItemsSource = "";
            serviceFulls.Clear();
            List<Service> services = db.Service.ToList();
            List<CheckIn> check = db.CheckIn.ToList();

            var service = from r in services
                          select new
                          {
                              ID = r.ID,
                              Name = r.Name,
                              Cost = Math.Round(r.Cost),
                          };

            foreach (var item in service)
            {
                serviceFulls.Add(new ServiceFull
                {
                    ID = item.ID,
                    Name = item.Name,
                    Cost = item.Cost,
                    DayStart = roomFor.DateCheckIn,
                    DayOver = roomFor.DateCheckOut,
                });
            }
            ListService.ItemsSource = serviceFulls;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            if (sf.Count > 0)
                ClearUpdate();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                if (sf.Count > 0)
                    ClearUpdate();
                else LoadData();
            }
            else
            {
                serviceFulls = serviceFulls.Where(c => c.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
                ListService.ItemsSource = serviceFulls;
            }
        }

        public void ClearUpdate()
        {
            LoadData();
            foreach (var j in sf)
                serviceFulls = serviceFulls.Where(x => x.ID != j.ID).ToList();
            ListService.ItemsSource = serviceFulls;
        }

        private void AddServiseButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDayServiceWindow takeDay = new TakeDayServiceWindow();

            var item = ListService.SelectedItem as ServiceFull;
            if (item != null)
            {
                takeDay.DayStartPicker.DisplayDateStart = item.DayStart;
                takeDay.DayStartPicker.DisplayDateEnd = item.DayOver;
                takeDay.DayOverPicker.DisplayDateStart = item.DayStart;
                takeDay.DayOverPicker.DisplayDateEnd = item.DayOver;

                if (takeDay.ShowDialog() == true)
                {

                    if(takeDay.DayStartPicker.SelectedDate.HasValue == true && takeDay.DayOverPicker.SelectedDate.HasValue == true)
                    {
                        item.DayStart = (DateTime)takeDay.DayStartPicker.SelectedDate;
                        item.DayOver = (DateTime)takeDay.DayOverPicker.SelectedDate;
                    }
                    else
                    {
                        item.DayStart = null;
                        item.DayOver = null;
                    }

                    takeDay.Close();

                    sf.Add(item);
                    ListService.ItemsSource = "";
                    serviceFulls = serviceFulls.Where(x => x.ID != item.ID).ToList();
                    ListService.ItemsSource = serviceFulls;
                    foreach (var i in sf)
                    {
                        if(i.Name == item.Name)
                        {
                            if (i.DayStart > i.DayOver)
                            {
                                sf.Remove(item);
                                ClearUpdate();
                                MessageBox.Show("Дата начала услуги не может быть позже даты окончания услуги");
                                break;
                            }
                            else if (i.DayStart == null || i.DayOver == null)
                            {
                                sf.Remove(item);
                                ClearUpdate();
                                MessageBox.Show("Дата начала или окончания услуги не может быть пуста");
                                break;
                            }
                            else
                                MessageBox.Show("Услуга добавлена");
                        }                       
                    }
                }
            }
            else
                MessageBox.Show("Чтобы добавить услугу, нажмите на элемент в списке");
        }

        private void BeforePageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CheckInFinichPage(id, visitors, roomFor, sf));
        }
    }
}
