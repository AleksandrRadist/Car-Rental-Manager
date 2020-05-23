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
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
    {
        DB_Access fun = new DB_Access();
        Repository rep = new Repository();
        public event Action UpdateClient;
        string di;
        string dil;
        string t;
        Dictionary<string, string> infodict;
        Dictionary<string, List<string>> attr = new Dictionary<string, List<string>>
        {
            ["Clients"] = new List<string>()
            {
                "name",
                "surname",
                "patronomic",
                "email",
                "phone",
                "gender",
                "birthdate",
                "address",
                "birthplace",
                "passport_series",
                "passport_number",
                "license_number",
                "license_first_issue_date",
                "license_date_of_issue",
                "license_date_end",
                "license_issue_place",
                "black_list_status"
            },
            ["Cars"] = new List<string>()
            {
                "model_id",
                "car_status_id",
                "station_id",
                "price_per_day",
                "car_number",
                "description"
            },
            ["Models"] = new List<string>()
            {
                "model_class_id",
                "model_name",
                "brand",
                "number_of_seats",
                "engine_power_hp",
                "engine_capacity_l",
                "maximum_speed_km/h",
                "trunk_size_l",
                "gasoline_type",
                "colour",
                "issue_year",
                "fuel_cunsumption_average_l",
                "transmission",
                "climate_control",
                "audio_system",
                "cruise_control",
                "rear_view_camera",
                "on_board_computer"
            },
            ["Classs"] = new List<string>()
            {
                "name",
                "min_age",
                "min_experience"
            },
            ["Employees"] = new List<string>()
            {
                "station_id",
                "salary_per_month_rub",
                "date_of_employment",
                "name",
                "surname",
                "patronomic",
                "email",
                "phone",
                "gender",
                "birthdate",
                "address",
                "place_of_birth",
                "passport_series",
                "passport_number",
                "snils",
                "inn"
            },
            ["Orders"] = new List<string>()
            {
                "client_id",
                "station_id_start",
                "station_id_end",
                "tariff_id",
                "order_status_id",
                "car_id",
                "date_start",
                "date_end",
                "cost",
                "payment_date",
                "order_date",
                "child_seat",
                "gps_navigator",
                "employee_id_start"
            },
            ["Invoices"] = new List<string>()
            {
                "total_cost",
                "employee_id"
            },
            ["Violations"] = new List<string>()
            {
                "vtype_id",
                "description",
                "fine_amount",
                "date",
                "payment_date",
                "order_id"
            },
            ["Tariffs"] = new List<string>()
            {
                "tariff_name",
                "minimum_day",
                "maximum_day",
                "price_multiplier",
                "mileage_limit"
            },
            ["Stations"] = new List<string>()
            {
                "city",
                "address",
                "phone"
            }
        };

        public EditClient(string id,  string table)
        {
            InitializeComponent();
            di = id;
            t = table;
            Dictionary<string, string> tblqr = new Dictionary<string, string>
            {
                ["Clients"] = "select Clients.client_id, surname, name, patronomic, email, phone, birthdate, address, birthplace, gender, " +
                "passport_series, passport_number, registration_date, black_list_status, license_number, date_of_issue as license_issue_date, date_end " +
                "as license_end_date, issue_place as license_issue_place, first_issue_date as license_first_issue from (Clients inner join " +
                $"Driver_license_info on Clients.license_id = Driver_license_info.license_id) inner join Client_info on Clients.client_id = Client_info.client_id where Clients.client_id = {di}",
                ["Cars"] = "select car_id, car_status_id, station_id, price_per_day," +
                $"car_number, model_id, description from Cars where car_id = {di}",
                ["Models"] = "select model_id, model_name, model_class_id, number_of_seats, brand, engine_power_hp, engine_capacity_l," +
                "maximum_speed_km_h, trunk_size_l, gasoline_type, colour, issue_year, fuel_consumption_average_l, transmission, climate_control," +
                $"audio_system, cruise_control, rear_view_camera, on_board_computer from Models where model_id = {di}",
                ["Model_class"] = $"select model_class_id, name, min_age, min_experience from Model_class model_class_id = {di}",
                ["Employees"] = "select Employees.employee_id, name, surname, patronomic, email, phone, birthdate, " +
                "address, place_of_birth as birthplace, gender, passport_series, passport_number, snils, inn, station_id, salary_per_month_rub as salary," +
                $" date_of_employment as employment_date from Employees inner join Employee_info on Employees.employee_id = Employee_info.employee_id where Employees.employee_id = {di}",
                ["Orders"] = "select order_id, client_id, station_id_start, station_id_end, tariff_id, order_status_id, car_id," +
                $" date_start, date_end, cost, payment_date, order_date, child_seat, gps_navigator, employee_id_start from Orders where order_id = {di}",
                ["Invoices"] = $"select order_id, total_cost, employee_id from Invoices",
                ["Violations"] = "select violation_id, vtype_id, fine_amount, date, payment_date, order_id, description from Violations " +
                $"where violation_id = {di}",
                ["Tariffs"] = $"select tariff_id, tariff_name as name, minimum_day, maximum_day, price_multiplier, mileage_limit from Tariffs where tariff_id = {di}",
                ["Stations"] = $"select station_id, city, address, phone from Stations where station_id = {di}"
            };
            infodict = tblqr;
            Update();
            optionBox.ItemsSource = attr[t];
        }
        public void Update()
        {
            DataTable table = fun.DataWithAdapt(infodict[t]);
            if (t == "Clients")
            {
                List<string> toDate = new List<string>()
                {
                "birthdate", "license_issue_date", "license_end_date", "license_first_issue"
                };
                Info.ItemsSource = rep.ConvertToDate(toDate, table).DefaultView;
            }
            else if (t == "Employees")
            {
                List<string> toDate = new List<string>{
                "birthdate", "employment_date"
                };
                Info.ItemsSource = rep.ConvertToDate(toDate, table).DefaultView;
            }
            else
                Info.ItemsSource = table.DefaultView;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string change;
            if (string.IsNullOrEmpty(changeBox.Text))
                change = "NULL";
            else
                change = "'" + changeBox.Text + "'";
            string option = optionBox.SelectedItem as string;
            if (option != null)
            {
                if (t == "Clients")
                {
                    if (option.Split("_")[0] == "license")
                    {
                        if (option != "license_number")
                            option = option.Remove(0, 8);
                        fun.ExecuteQuery($"Update Driver_license_info set {option} = {change} where license_id = {dil}");
                    }
                    else if (option == "black_list_status")
                        fun.ExecuteQuery($"Update Clients set black_list_status = {change} where client_id = {di}");
                    else
                        fun.ExecuteQuery($"Update Client_info set {option} = {change} where client_id = {di}");
                }
                else if (t == "Employees")
                {
                    if (option == "station_id" || option == "salary_per_month_rub" || option == "date_of_employment")
                        fun.ExecuteQuery($"Update Employees set {option} = {change} where employee_id = {di}");
                    else
                        fun.ExecuteQuery($"Update Employee_info set {option} = {change} where employee_id = {di}");
                }
                else
                {
                    DataTable tab = fun.DataWithAdapt(infodict[t]);
                    string s = $"Update {t} set {option} = {change} where {tab.Columns[0].ColumnName} = {di}";
                    fun.ExecuteQuery(s);
                }
                MessageBox.Show("Information has been changed");
                UpdateClient?.Invoke();
                Update();
            }
            else
                MessageBox.Show("Please choose an attribute");
        }
    }
}
