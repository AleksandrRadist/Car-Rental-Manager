using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using car_rental;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string st;
            if (string.IsNullOrEmpty(station.Text))
                st = "NULL";
            else
                st = station.Text;
            fun.ExecuteQuery($"exec dbo.AddEmployee {st}, {salary.Text}, '{email.Text}', '{phone.Text}', '{address.Text}', '{name.Text}'," +
                $" '{surname.Text}', '{patronomic.Text}', '{birthplace.Text}', '{snils.Text}', '{inn.Text}', '{pseries.Text}', '{pnum.Text}', '{gender.Text}'," +
                $" '{birthdate.Text.Replace(".", "-")}', '{date.Text.Replace(".", "-")}'");
            MessageBox.Show("Employee has been added");
            UpdateInfo?.Invoke();
            Close();
        }
    }
}
