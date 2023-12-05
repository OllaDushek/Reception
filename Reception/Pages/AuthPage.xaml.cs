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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();
        int id;

        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            var Login = LoginBox.Text;
            var Password = PasswordBox.Password;
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
                MessageBox.Show("Вы не заполнили все поля");
            else
            {
                if (db.Worker.Any(u => u.Login == Login) == true)
                {
                    foreach (var user in db.Worker)
                    {
                        if (user.Login == Login)
                        {
                            if (user.Password == Password)
                                if (user.RoleID == 1)
                                {
                                    id = user.ID;
                                    NavigationService.Navigate(new AdminMainPage(id));
                                }
                                else
                                {
                                    id = user.ID;
                                    NavigationService.Navigate(new WorkerMainPage(id));
                                }
                            else
                                MessageBox.Show("Вы ввели неправильный пароль");
                        }
                    }
                }
                else
                    MessageBox.Show("Такого пользователя не существует");
            }
            
        }
    }
}
