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
using System.Windows.Shapes;

namespace Reception.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для TakeDopServiceWindow.xaml
    /// </summary>
    public partial class TakeDopServiceWindow : Window
    {

        ReseptionEntities db = new ReseptionEntities();

        public TakeDopServiceWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void IDRoomBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<CheckIn> checkIns = db.CheckIn.ToList();
            List<Room> rooms = db.Room.ToList();
            List<ListDopService> listDopServices = db.ListDopService.ToList();
            List<Service> services = db.Service.ToList();
            string item = NameServiceBlock.Text;

            checkIns = checkIns.Where(x => x.StatusID == 1).ToList();

            int textcombo = Convert.ToInt32(IDRoomBox.SelectedItem);

            if (IDRoomBox.SelectedItem != null)
            {
                DayStartPicker.BlackoutDates.Clear();
                DayOverPicker.BlackoutDates.Clear();
                TakeDateForServicePanel.Visibility = Visibility.Visible;
                foreach (var i in rooms)
                {
                    foreach (var j in checkIns)
                    {
                        if (textcombo == i.RoomNumber)
                        {
                            DayStartPicker.DisplayDateStart = j.DateCheckIn;
                            DayStartPicker.DisplayDateEnd = j.DateCheckOut;
                            DayOverPicker.DisplayDateStart = j.DateCheckIn;
                            DayOverPicker.DisplayDateEnd = j.DateCheckOut;
                        }
                    }
                }

                foreach (var i in listDopServices)
                {
                    foreach (var j in rooms)
                    {
                        foreach (var c in checkIns)
                        {
                            foreach(var s in services)
                            {
                                if (i.CheckInID == c.ID && i.ServiceID == s.ID && j.ID == c.RoomID && s.Name == item)
                                {
                                    DayStartPicker.BlackoutDates.Add(new CalendarDateRange(i.DayStart, i.DayOver));
                                    DayOverPicker.BlackoutDates.Add(new CalendarDateRange(i.DayStart, i.DayOver));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
