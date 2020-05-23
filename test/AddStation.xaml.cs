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
    /// Логика взаимодействия для AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddStation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fun.ExecuteQuery($"exec dbo.AddStation '{city.Text}', '{address.Text}', '{phone.Text}'");
            MessageBox.Show("Station has been added");
            UpdateInfo?.Invoke();
            Close();
        }
    }
}
