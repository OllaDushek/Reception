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
    /// Логика взаимодействия для AddWorkerPage.xaml
    /// </summary>
    public partial class AddWorkerPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        int idwor;

        public AddWorkerPage(int idwor)
        {
            InitializeComponent();
            this.idwor = idwor;
        }

        public int CatchError()
        {
            if (String.IsNullOrWhiteSpace(LastNameBox.Text) || String.IsNullOrWhiteSpace(FirstNameBox.Text) || String.IsNullOrWhiteSpace(PatronymicBox.Text)
                || String.IsNullOrWhiteSpace(LoginBox.Text) || String.IsNullOrWhiteSpace(PasswordBox.Text) || RoleBox.SelectedItem == null
                || BirthBox.SelectedDate.HasValue == false)
                return 1;
            else if (!PhoneBox.Text.All(char.IsDigit))
                return 2;
            else if (PhoneBox.Text.StartsWith("7") == false)
                return 3;
            else return 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {           
            RoleBox.ItemsSource = db.Role.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (CatchError())
                {
                case 1:
                    MessageBox.Show("Вы не заполнили все поля");
                    break;
                case 2:
                    MessageBox.Show("Вы неправильно заполнили поле номера телефона");
                    break;
                case 3:
                    MessageBox.Show("Вы неправильно ввели номер телефона. Он должен начинаться с 7");
                    break;
                case 0:

                    Worker worker = new Worker();
                    worker.Phone = PhoneBox.Text;
                    worker.LastName = LastNameBox.Text;
                    worker.FirstName = FirstNameBox.Text;
                    worker.Patronymic = PatronymicBox.Text;
                    worker.Login = LoginBox.Text[0].ToString().ToUpper();
                    worker.Password = PasswordBox.Text;
                    if ((RoleBox.SelectedItem as Role).Name == "Админ")
                        worker.RoleID = 1;
                    else worker.RoleID = 2;
                    worker.Bith = (DateTime)BirthBox.SelectedDate;
                    db.Worker.Add(worker);
                    db.SaveChanges();
                    MessageBox.Show("Данные были добавлены");
                    break;
                }
                NavigationService.Navigate(new WorkerForAdminPage(idwor));
            }
            catch
            {
                MessageBox.Show("Такой логин уже существует");
            }
}
    }
}
