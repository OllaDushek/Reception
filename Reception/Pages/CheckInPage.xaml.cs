using Reception.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для CheckInPage.xaml
    /// </summary>
    public partial class CheckInPage : Page
    {

        ReseptionEntities db = new ReseptionEntities();

        List<Visitor> visitors = new List<Visitor>();

        int num;
        int id;
        bool okey = true;

        public CheckInPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public void LoadData()
        {
            ListNewCustomer.ItemsSource = null;
            if(visitors.Count > 0)
                ListNewCustomer.ItemsSource = visitors.ToList();
            else ListNewCustomer.Visibility = Visibility.Collapsed;
        }

        public Visitor GetVisitor()
        {
            Visitor visitor = new Visitor();
            visitor.LastName = LastNameBox.Text;
            visitor.FirstName = FirstNameBox.Text;
            visitor.Patronymic = PatronymicBox.Text;
            visitor.Bith = Convert.ToDateTime(BirthBox.Text);
            visitor.NumberPasport = NumberBox.Text;
            visitor.SeriesPassport = SerialBox.Text;
            visitor.Phone = PhoneBox.Text;
            return visitor;
        }

        public bool GetOkey(Visitor visitor)
        {
            List<Visitor> v = new List<Visitor>();
            v = db.Visitor.ToList();

            foreach (var i in v)
            {
                if (i.LastName == visitor.LastName && i.FirstName == visitor.FirstName && i.Patronymic == visitor.Patronymic && i.Bith == visitor.Bith
                    && i.NumberPasport == visitor.NumberPasport && i.SeriesPassport == visitor.SeriesPassport && i.Phone == visitor.Phone)
                {
                    okey = false;
                    break;
                }
                else
                    okey = true;
            }

            return okey;
        }

        public void AddVisitor()
        {
            try
            {
                Visitor visitor = new Visitor();
                visitor = GetVisitor();
                visitors.Add(visitor);
                okey = GetOkey(visitor);
                if (okey == true)
                {
                    db.Visitor.Add(visitor);
                    db.SaveChanges();
                }
                else visitors.Remove(visitor);
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        public void CleartextBox()
        {
            LastNameBox.Clear();
            FirstNameBox.Clear();
            PatronymicBox.Clear();
            PhoneBox.Clear();
            SerialBox.Clear();
            NumberBox.Clear();
            BirthBox.Text = null;
        }

        public int CatchError()
        {
            if (String.IsNullOrWhiteSpace(LastNameBox.Text) || String.IsNullOrWhiteSpace(FirstNameBox.Text) || String.IsNullOrWhiteSpace(PatronymicBox.Text)
                || String.IsNullOrWhiteSpace(SerialBox.Text) || String.IsNullOrWhiteSpace(NumberBox.Text) || String.IsNullOrWhiteSpace(PhoneBox.Text)
                || BirthBox.SelectedDate.HasValue == false)
                return 1;
            else if (!SerialBox.Text.All(char.IsDigit) || !NumberBox.Text.All(char.IsDigit) || !PhoneBox.Text.All(char.IsDigit))
                return 2;
            else if (SerialBox.Text.Length < 4 || NumberBox.Text.Length < 6 || PhoneBox.Text.Length < 11)
                return 4;
            else if (PhoneBox.Text.StartsWith("7") == false)
                return 3;
            else return 0;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (visitors.Count > 0)
                NavigationService.Navigate(new CheckInNumberPage(id, visitors));
            else
            {
                switch (CatchError())
                {
                    case 1:
                        MessageBox.Show("Вы не заполнили все поля");
                        break;
                    case 2:
                        MessageBox.Show("Вы неправильно заполнили поля ввода серии/номера паспорта или номера телефона");
                        break;
                    case 3:
                        MessageBox.Show("Вы неправильно ввели номер телефона. Он должен начинаться с 7");
                        break;
                    case 4:
                        MessageBox.Show("Вы неправильно записали серию/номер паспорта или номер телефона");
                        break;
                    case 0:
                        AddVisitor();
                        NavigationService.Navigate(new CheckInNumberPage(id, visitors));
                        break;
                }
            }
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            switch (CatchError())
            {
                case 1:
                    MessageBox.Show("Вы не заполнили все поля");
                    break;
                case 2:
                    MessageBox.Show("Вы неправильно заполнили поля ввода серии/номера паспорта или номера телефона");
                    break;
                case 3:
                    MessageBox.Show("Вы неправильно ввели номер телефона. Он должен начинаться с 7");
                    break;
                case 4:
                    MessageBox.Show("Вы неправильно записали серию/номер паспорта или номер телефона");
                    break;
                case 0:
                    Visitor visitor = new Visitor();
                    visitor = GetVisitor();
                    okey = GetOkey(visitor);
                    if (okey == true)
                    {
                        AddVisitor();
                        CleartextBox();
                        ListNewCustomer.Visibility = Visibility.Visible;
                        LoadData();
                    }
                    else
                    {
                        CleartextBox();
                        ListNewCustomer.Visibility = Visibility.Visible;
                        LoadData();
                    }
                    break;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {           
            if (MessageBox.Show($"Вы действительно хотете удалить - этого посетителя?",
                            "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var item = ListNewCustomer.SelectedItem as Visitor;
                    if (item != null)
                    {
                        db.Visitor.Remove(item);
                        db.SaveChanges();
                        visitors.Remove(item);
                        CleartextBox();
                        LoadData();
                        MessageBox.Show("Посетитель был удален");
                    }
                    else
                        MessageBox.Show("Чтобы удалить посетителя, нажмите на элемент в списке");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ListNewCustomer.SelectedItem as Visitor;
            if (item != null)
            {
                NextPageButton.Visibility = Visibility.Collapsed;
                UpdateCustomerButton.Visibility = Visibility.Visible;
                AddNewCustomer.IsEnabled = false;
                LastNameBox.Text = item.LastName;
                FirstNameBox.Text = item.FirstName;
                PatronymicBox.Text = item.Patronymic;
                BirthBox.SelectedDate = item.Bith;
                SerialBox.Text = item.SeriesPassport;
                NumberBox.Text = item.NumberPasport;
                PhoneBox.Text = item.Phone;
                num = item.ID;
                ListNewCustomer.Visibility = Visibility.Collapsed;
            }
            else
                MessageBox.Show("Чтобы изменить данные посетителя, нажмите на элемент в списке");
        }

        private void BronButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BronPage(id));
        }

        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            switch (CatchError())
            {
                case 1:
                    MessageBox.Show("Вы не заполнили все поля");
                    break;
                case 2:
                    MessageBox.Show("Вы неправильно заполнили поля ввода серии/номера паспорта или номера телефона");
                    break;
                case 3:
                    MessageBox.Show("Вы неправильно ввели номер телефона. Он должен начинаться с 7");
                    break;
                case 4:
                    MessageBox.Show("Вы неправильно записали серию/номер паспорта или номер телефона");
                    break;
                case 0:
                    var uRow = db.Visitor.Where(w => w.ID == num).FirstOrDefault();
                    uRow.Phone = PhoneBox.Text;
                    uRow.LastName = LastNameBox.Text;
                    uRow.FirstName = FirstNameBox.Text;
                    uRow.Patronymic = PatronymicBox.Text;
                    uRow.SeriesPassport = SerialBox.Text;
                    uRow.NumberPasport = NumberBox.Text;
                    uRow.Bith = (DateTime)BirthBox.SelectedDate;
                    db.SaveChanges();
                    var row = visitors.Where(x => x.ID == num).FirstOrDefault();
                    row.Phone = PhoneBox.Text;
                    row.LastName = LastNameBox.Text;
                    row.FirstName = FirstNameBox.Text;
                    row.Patronymic = PatronymicBox.Text;
                    row.SeriesPassport = SerialBox.Text;
                    row.NumberPasport = NumberBox.Text;
                    row.Bith = (DateTime)BirthBox.SelectedDate;
                    CleartextBox();
                    LoadData();
                    UpdateCustomerButton.Visibility = Visibility.Collapsed;
                    NextPageButton.Visibility = Visibility.Visible;
                    ListNewCustomer.Visibility = Visibility.Visible;
                    AddNewCustomer.IsEnabled = true;
                    break;
            }
        }
    }
}