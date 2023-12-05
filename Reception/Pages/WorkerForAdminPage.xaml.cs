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
    /// Логика взаимодействия для WorkerForAdminPage.xaml
    /// </summary>
    public partial class WorkerForAdminPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        int id;

        public WorkerForAdminPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public void LoadData()
        {
            ListWorker.ItemsSource = db.Worker.Where(x => x.ID != id).ToList();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddWorkerPage(id));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ListWorker.SelectedItem as Worker;
            if (item != null)
            {
                NavigationService.Navigate(new UdateWorkerPage(item, id));
            }
            else
                MessageBox.Show("Чтобы изменить данные сотрудника, нажмите на элемент в списке");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотете удалить - этого сотрудника?",
                            "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var item = ListWorker.SelectedItem as Worker;
                    if (item != null)
                    {
                        db.Worker.Remove(item);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Сотрудник был удален");
                    }
                    else
                        MessageBox.Show("Чтобы удалить сотрудника, нажмите на элемент в списке");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
