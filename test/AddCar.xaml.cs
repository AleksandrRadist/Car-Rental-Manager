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
    /// Логика взаимодействия для AddCar.xaml
    /// </summary>
    public partial class AddCar : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddCar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string st;
            if (station == null)
                st = "NULL";
            else
                st = station.Text;
            fun.ExecuteQuery($"exec dbo.AddCar {status.Text}, {st}, {model.Text}, {price.Text}, '{num.Text}', '{desc.Text}'");
            MessageBox.Show("Car has been added");
            UpdateInfo?.Invoke();
            Close();
        }
    }
}
