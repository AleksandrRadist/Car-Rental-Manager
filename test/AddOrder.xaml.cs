using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        DB_Access fun = new DB_Access();
        public event Action UpdateInfo;
        public AddOrder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable tbl;
            string clientId = client.Text;
            tbl = fun.DataWithAdapt($"select black_list_status from Clients where client_id = {clientId}");
            if (tbl.Rows[0].Field<bool>("black_list_status"))
            {
                MessageBox.Show("This client is in black list");
                cost.Text = null;
                return;
            }
            tbl = fun.DataWithAdapt($"select car_status_id from Cars where car_id = {car.Text}");
            if (tbl.Rows[0].Field<int>("car_status_id") != 1)
            {
                MessageBox.Show("Car is not available");
                cost.Text = null;
                return;
            }
            tbl = fun.DataWithAdapt($"select min_age, min_experience from (Model_class inner join Models on Model_class.model_class_id = Models.model_class_id)" +
                $" inner join Cars on Cars.model_id = Models.model_id where Cars.car_id = {car.Text}");
            int cage = tbl.Rows[0].Field<int>("min_age");
            int cexp = tbl.Rows[0].Field<int>("min_experience");
            tbl = fun.DataWithAdapt("select birthdate, first_issue_date from (Clients inner join Client_info on Clients.client_id = Client_info.client_id)" +
                $" inner join Driver_license_info on Clients.license_id = Driver_license_info.license_id where Clients.client_id = {client.Text}");
            DateTime now = DateTime.Now;
            if (tbl.Rows[0].Field<DateTime>("birthdate").AddYears(cage) > now || tbl.Rows[0].Field<DateTime>("first_issue_date").AddYears(cexp) > now)
            {
                MessageBox.Show("Client does not meet requirements for requested car");
                cost.Text = null;
                return;
            }
            tbl = fun.DataWithAdapt($"select station_id from Employees where employee_id = {employee.Text}");
            if (tbl.Rows[0].Field<int>("station_id").ToString() != station1.Text)
            {
                MessageBox.Show("Requested employee is not positioned on requested station");
                cost.Text = null;
                return;
            }
            string payd;
            if (string.IsNullOrEmpty(paydate.Text))
                payd = "NULL";
            else
                payd = "'" + paydate.Text + "'";
            fun.ExecuteQuery($"exec dbo.AddOrder {client.Text}, {station1.Text}, {station2.Text}, {tariff.Text}, {car.Text}, " +
                $"'{date1.Text}', '{date2.Text}', {payd}, {child.Text}, {gps.Text}, {employee.Text}, '{com.Text}'");
            MessageBox.Show("Order has been added");
            UpdateInfo?.Invoke();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataTable tbl;
            TimeSpan len = DateTime.Parse(date2.Text) - DateTime.Parse(date1.Text);
            int days = len.Days + 1;
            tbl = fun.DataWithAdapt($"select * from Tariffs where tariff_id = {tariff.Text}");
            if  (tbl.Rows[0].Field<int>("minimum_day") > days || days > tbl.Rows[0].Field<int>("maximum_day"))
            {
                MessageBox.Show("Requested tariff is not suitable for requested period");
                return;
            }
            decimal mult = tbl.Rows[0].Field<decimal>("price_multiplier");
            tbl = fun.DataWithAdapt($"select price_per_day from Cars where car_id = {car.Text}");
            decimal price = tbl.Rows[0].Field<decimal>("price_per_day");
            if (child.Text == "1")
                price += 200;
            if (gps.Text == "1")
                price += 200;
            price = days * price * mult;
            cost.Text = price.ToString();
        }
    }
}
