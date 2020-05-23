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
    /// Логика взаимодействия для AddViolation.xaml
    /// </summary>
    public partial class AddViolation : Window
    
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddViolation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pay;
            if (string.IsNullOrEmpty(pdate.Text))
                pay = "NULL";
            else
                pay = "'" + pdate.Text + "'";
            fun.ExecuteQuery($"exec AddViolation {order.Text}, {type.Text}, '{desc.Text}', {fine.Text}, '{date.Text}', {pay}");
            UpdateInfo?.Invoke();
            MessageBox.Show("Violation has been added");
            Close();
        }
    }
}
