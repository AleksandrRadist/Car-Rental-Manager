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
    /// Логика взаимодействия для AddModel.xaml
    /// </summary>
    public partial class AddModel : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddModel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fun.ExecuteQuery($"exec dbo.AddModel '{name.Text}', {seat.Text}, '{brand.Text}', '{power.Text}', '{cap.Text}', '{speed.Text}', {trunk.Text}," +
                $" '{gaz.Text}', '{colour.Text}', {year.Text}, '{consump.Text}', '{trans.Text}', {clas.Text}, {climate.Text}, {audio.Text}, {cruise.Text}," +
                $" {rear.Text}, {comp.Text}");
            MessageBox.Show("Model has been added");
            UpdateInfo?.Invoke();
            Close();
        }
    }
}
