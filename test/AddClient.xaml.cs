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
    
    public partial class AddClient : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateClient;
        public AddClient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fun.ExecuteQuery($"exec dbo.AddClient '{name.Text}', '{surname.Text}', '{patronomic.Text}', '{email.Text}', '{phone.Text}', '{address.Text}', " +
                $"'{birthplace.Text}', '{pseries.Text}', '{pnum.Text}', '{birthdate.Text.Replace(".", "-")}', '{gender.Text}', '{sis.Text.Replace(".", "-")}', '{eis.Text.Replace(".", "-")}', '{ip.Text}'," +
                $" '{lnum.Text}', '{fis.Text.Replace(".", "-")}'");
            MessageBox.Show("Client has been added");
            UpdateClient?.Invoke();
            Close();
        }
    }
}
