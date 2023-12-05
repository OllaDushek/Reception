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
    /// Логика взаимодействия для UdateWorkerPage.xaml
    /// </summary>
    public partial class UdateWorkerPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        private Worker id;
        int idwor;

        public UdateWorkerPage(Worker id, int idwor)
        {
            InitializeComponent();
            this.id = id;
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
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
                        var uRow = db.Worker.Where(w => w.ID == id.ID).FirstOrDefault();
                        uRow.Phone = PhoneBox.Text;
                        uRow.LastName = LastNameBox.Text;
                        uRow.FirstName = FirstNameBox.Text;
                        uRow.Patronymic = PatronymicBox.Text;
                        uRow.Login = LoginBox.Text.ToUpper();
                        uRow.Password = PasswordBox.Text;
                        if ((RoleBox.SelectedItem as Role).Name == "Админ")
                            uRow.RoleID = 1;
                        else uRow.RoleID = 2;
                        uRow.Bith = (DateTime)BirthBox.SelectedDate;
                        db.SaveChanges();
                        MessageBox.Show("Данные были изменены");
                        break;
                }
                NavigationService.Navigate(new WorkerForAdminPage(idwor));
            }
            catch
            {
                MessageBox.Show("Такой логин уже существует");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Role> roles = db.Role.ToList();
            LastNameBox.Text = id.LastName;
            FirstNameBox.Text = id.FirstName;
            PatronymicBox.Text = id.Patronymic;
            PasswordBox.Text = id.Password;
            LoginBox.Text = id.Login;
            PhoneBox.Text = id.Phone;
            BirthBox.SelectedDate = id.Bith;
            RoleBox.ItemsSource = db.Role.ToList();
            foreach(var i in roles)
            {
                if (i.ID == id.RoleID && id.RoleID == 2)
                    RoleBox.SelectedIndex = 1;
                else if (i.ID == id.RoleID && id.RoleID == 1)
                    RoleBox.SelectedIndex = 0;
            }
        }
    }
}
