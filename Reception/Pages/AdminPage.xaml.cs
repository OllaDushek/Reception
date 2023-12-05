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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {

        int id;

        public AdminPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void WorkerButton_Click(object sender, RoutedEventArgs e)
        {
            frameWork.NavigationService.Navigate(new WorkerForAdminPage(id));
        }

        private void RoomButton_Click(object sender, RoutedEventArgs e)
        {
            frameWork.NavigationService.Navigate(new RoomForWorkerPage(id));
        }

        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminMainPage(id));
        }

        private void OutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
