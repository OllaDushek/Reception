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
    /// Логика взаимодействия для WorkerMainPage.xaml
    /// </summary>
    public partial class WorkerMainPage : Page
    {

        int id;

        public WorkerMainPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            frameWork.NavigationService.Navigate(new CheckInPage(id));
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            frameWork.NavigationService.Navigate(new ServicePage());
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            frameWork.NavigationService.Navigate(new CheckOutPage(id));
        }

        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDocumentAllWindow createDocument = new CreateDocumentAllWindow();
            CreateDocAllClass allClass = new CreateDocAllClass();

            if (createDocument.ShowDialog() == true)
            {
                if(createDocument.DayStartPicker.SelectedDate.HasValue == true && createDocument.DayOverPicker.SelectedDate.HasValue == true)
                    allClass.CreateDoc(id, (DateTime)createDocument.DayStartPicker.SelectedDate, (DateTime)createDocument.DayOverPicker.SelectedDate);  
                createDocument.Close();
            }
        }
    }
}
