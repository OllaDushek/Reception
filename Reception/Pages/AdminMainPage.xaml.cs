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
    /// Логика взаимодействия для AdminMainPage.xaml
    /// </summary>
    public partial class AdminMainPage : Page
    {

        int id;

        public AdminMainPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void ForWorkerButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new WorkerMainPage(id));
        }

        private void ForAdminButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(id));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
