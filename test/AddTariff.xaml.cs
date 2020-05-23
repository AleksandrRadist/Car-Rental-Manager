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
    /// Логика взаимодействия для AddTariff.xaml
    /// </summary>
    public partial class AddTariff : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddTariff()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fun.ExecuteQuery($"exec dbo.AddTariff '{name.Text}', {min.Text}, {max.Text}, '{mult.Text}', {lim.Text}");
            MessageBox.Show("Tariff has been added");
            UpdateInfo?.Invoke();
            Close();
        }
    }
}
