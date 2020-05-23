using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using car_rental;


namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB_Access fun = new DB_Access();
        Repository rep = new Repository();
        Dictionary<string, string> report_dict = new Dictionary<string, string>
        {
            ["Tariffs usage report"] = "select tariff_name, count(*) as number_of_orders from Orders inner join Tariffs on Orders.tariff_id = Tariffs.tariff_id group by tariff_name order by number_of_orders desc/group/order_date",
            ["Model income report"] = "select Models.model_id, count(*) as number_of_orders, sum(cost) as total_income from (Models inner join Cars on Models.model_id = Cars.model_id) " +
            "inner join (select * from Orders where order_status_id > 3) as a on a.car_id = Cars.car_id group by Models.model_id order by total_income desc/group/order_date",
            ["Employee issued orders report"] = "select Employees.employee_id, count(*) as number_of_orders from Employees inner join Orders on Employees.employee_id = Orders.employee_id_start " +
            "group by Employees.employee_id order by number_of_orders desc/group/order_date",
            ["Employee ended orders report"] = "select Employees.employee_id, count(*) as number_of_orders from Employees inner join Invoices on Employees.employee_id = Invoices.employee_id " +
            "group by Employees.employee_id order by number_of_orders desc/group/order_date"
        };
        
        public MainWindow() 
        {
            InitializeComponent();
            Update();
            optionReport.ItemsSource = report_dict.Keys;

        }
        
        public void Update()
        {
            fun = new DB_Access();
            DataTable table = fun.DataWithAdapt("select Clients.client_id as id, surname, name, patronomic, email, phone, birthdate, address, birthplace, gender, passport_series, " +
                "passport_number, registration_date, black_list_status, Clients.license_id as license_id, license_number, date_of_issue as license_issue_date, date_end as license_end_date, " +
                "issue_place as license_issue_place, first_issue_date as license_first_issue from (Clients inner join Driver_license_info " +
                "on Clients.license_id = Driver_license_info.license_id) inner join Client_info on Clients.client_id = Client_info.client_id");
            List<string> toDate = new List<string>()
            {
                "birthdate", "license_issue_date", "license_end_date", "license_first_issue"
            };
            client_list.ItemsSource = rep.ConvertToDate(toDate, table).DefaultView;
            car_list.ItemsSource = fun.DataWithAdapt("select car_id as id, car_status.name as car_status, station_id, price_per_day," +
                "car_number, model_id as model_id, description from Cars inner join Car_status on Cars.car_status_id = " +
                "Car_status.car_status_id").DefaultView;
            model_list.ItemsSource = fun.DataWithAdapt("select model_id as id, model_name, model_class_id, number_of_seats, brand, engine_power_hp, engine_capacity_l," +
                "maximum_speed_km_h, trunk_size_l, gasoline_type, colour, issue_year, fuel_consumption_average_l, transmission, climate_control," +
                "audio_system, cruise_control, rear_view_camera, on_board_computer from Models").DefaultView;
            class_list.ItemsSource = fun.DataWithAdapt("select model_class_id as id, name, min_age, min_experience from Model_class").DefaultView;
            table = fun.DataWithAdapt("select Employees.employee_id as id, name, surname, patronomic, email, phone, birthdate, " +
                "address, place_of_birth as birthplace, gender, passport_series, passport_number, snils, inn, station_id, salary_per_month_rub as salary," +
                " date_of_employment as employment_date from Employees inner join Employee_info on Employees.employee_id = Employee_info.employee_id");
            toDate = new List<string>{
                "birthdate", "employment_date"
            };
            employee_list.ItemsSource = rep.ConvertToDate(toDate, table).DefaultView;
            order_list.ItemsSource = fun.DataWithAdapt("select order_id as id, client_id, station_id_start, station_id_end, tariff_name, Order_status.name, car_id," +
                " date_start, date_end, cost, payment_date, order_date, child_seat, gps_navigator, employee_id_start, comments from (Orders inner join" +
                " Tariffs on Orders.tariff_id = Tariffs.tariff_id) inner join Order_status on Order_status.order_status_id = Orders.order_status_id").DefaultView;
            invoice_list.ItemsSource = fun.DataWithAdapt("select order_id, total_cost, employee_id from Invoices").DefaultView;
            violation_list.ItemsSource = fun.DataWithAdapt("select violation_id as id, Violation_type.name as type, fine_amount, date, payment_date, order_id, description from Violations " +
                "inner join Violation_type on Violations.vtype_id = Violation_type.vtype_id").DefaultView;
            tariff_list.ItemsSource = fun.DataWithAdapt("select tariff_id as id, tariff_name as name, minimum_day, maximum_day, price_multiplier, mileage_limit from Tariffs").DefaultView;
            station_list.ItemsSource = fun.DataWithAdapt("select station_id as id, city, address, phone from Stations").DefaultView;
        }
        
        private void AddClient_Click(object sender, RoutedEventArgs e) 
        {
            var winAddCl = new AddClient();
            winAddCl.UpdateClient += Update;
            winAddCl.Show();

        }
        public void Edit(DataRowView ro, string t)
        {
            if (ro != null)
            {
                string id = ro.Row[0].ToString();
                var winEditCl = new EditClient(id, t);
                winEditCl.UpdateClient += Update;
                winEditCl.Show();
            }
            else
                MessageBox.Show("Please choose an object");
        }
        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void SearchClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchClient.Text))
                return;
            client_list.ItemsSource = fun.DataWithAdapt("select Clients.client_id as id, surname, name, patronomic, email, phone, birthdate, address, birthplace, gender, passport_series, " +
                "passport_number, registration_date, black_list_status, Clients.license_id as license_id, license_number, date_of_issue as license_issue_date, date_end as license_end_date, " +
                "issue_place as license_issue_place, first_issue_date as license_first_issue from (Clients inner join Driver_license_info " +
                $"on Clients.license_id = Driver_license_info.license_id) inner join Client_info on Clients.client_id = Client_info.client_id where Clients.client_id = {searchClient.Text}").DefaultView;
        }
        private void SearchCar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchCar.Text))
                return;
            car_list.ItemsSource = fun.DataWithAdapt("select car_id as id, car_status.name as car_status, station_id, price_per_day," +
                "car_number, model_id as model_id, description from Cars inner join Car_status on Cars.car_status_id = " +
                $"Car_status.car_status_id where car_id = {searchCar.Text}").DefaultView;
        }
        private void SearchModel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchModel.Text))
                return;
            model_list.ItemsSource = fun.DataWithAdapt("select model_id as id, model_name, model_class_id, number_of_seats, brand, engine_power_hp, engine_capacity_l," +
                "maximum_speed_km_h, trunk_size_l, gasoline_type, colour, issue_year, fuel_consumption_average_l, transmission, climate_control," +
                $"audio_system, cruise_control, rear_view_camera, on_board_computer from Models where model_id = {searchModel.Text}").DefaultView;
        }
        private void SearchOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchOrder.Text))
                return;
            order_list.ItemsSource = fun.DataWithAdapt("select order_id as id, client_id, station_id_start, station_id_end, tariff_name, Order_status.name, car_id," +
                " date_start, date_end, cost, payment_date, order_date, child_seat, gps_navigator, employee_id_start, comments from (Orders inner join" +
                $" Tariffs on Orders.tariff_id = Tariffs.tariff_id) inner join Order_status on Order_status.order_status_id = Orders.order_status_id where order_id = {searchOrder.Text}").DefaultView;
        }
        private void SearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchInvoice.Text))
                return;
            invoice_list.ItemsSource = fun.DataWithAdapt($"select order_id, total_cost, employee_id from Invoices where order_id = {searchInvoice.Text}").DefaultView;
        }
        private void SearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchEmployee.Text))
                return;
            employee_list.ItemsSource = fun.DataWithAdapt("select Employees.employee_id as id, name, surname, patronomic, email, phone, birthdate, " +
                "address, place_of_birth as birthplace, gender, passport_series, passport_number, snils, inn, station_id, salary_per_month_rub as salary," +
                " date_of_employment as employment_date from Employees inner join Employee_info on Employees.employee_id = Employee_info.employee_id" +
                $" where Employees.employee_id = {searchEmployee.Text}").DefaultView;
        }
        private void SearchClass_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchClass.Text))
                return;
            class_list.ItemsSource = fun.DataWithAdapt($"select model_class_id as id, name, min_age, min_experience from Model_class where model_class_id = {searchClass.Text}").DefaultView;
        }
        private void SearchStation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchStation.Text))
                return;
            station_list.ItemsSource = fun.DataWithAdapt($"select station_id as id, city, address, phone from Stations where station_id = {searchStation.Text}").DefaultView;
        }
        private void SearchTariff_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchTariff.Text))
                return;
            tariff_list.ItemsSource = fun.DataWithAdapt($"select tariff_id as id, tariff_name as name, minimum_day, maximum_day, price_multiplier, mileage_limit from Tariffs where tariff_id = {searchTariff.Text}").DefaultView;
        }
        private void SearchViolation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchViolation.Text))
                return;
            violation_list.ItemsSource = fun.DataWithAdapt("select violation_id as id, Violation_type.name as type, fine_amount, date, payment_date, order_id, description from Violations " +
                $"inner join Violation_type on Violations.vtype_id = Violation_type.vtype_id where violation_id = {searchViolation.Text}").DefaultView;
        }
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView ro = client_list.SelectedItem as DataRowView;
            Edit(ro, "Clients");
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            var winACar = new AddCar();
            winACar.UpdateInfo += Update;
            winACar.Show();
        }

        private void AddModel_Click(object sender, RoutedEventArgs e)
        {
            var winAModel = new AddModel();
            winAModel.UpdateInfo += Update;
            winAModel.Show();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var winAEmp = new AddEmployee();
            winAEmp.UpdateInfo += Update;
            winAEmp.Show();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var winAOrder = new AddOrder();
            winAOrder.UpdateInfo += Update;
            winAOrder.Show();
        }


        private void AddVio_Click(object sender, RoutedEventArgs e)
        {
            var winAVio = new AddViolation();
            winAVio.UpdateInfo += Update;
            winAVio.Show();
        }

        private void AddTariff_Click(object sender, RoutedEventArgs e)
        {
            var winATar = new AddTariff();
            winATar.UpdateInfo += Update;
            winATar.Show();
        }

        private void AddStation_Click_1(object sender, RoutedEventArgs e)
        {
            var winASta = new AddStation();
            winASta.UpdateInfo += Update;
            winASta.Show();
        }

        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            var winAClass = new AddClass();
            winAClass.UpdateInfo += Update;
            winAClass.Show();
        }
        
        private void EditTar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView ro = tariff_list.SelectedItem as DataRowView;
            Edit(ro, "Tariffs");
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView ro = car_list.SelectedItem as DataRowView;
            Edit(ro, "Cars");
        }

        private void EditModel_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView ro = model_list.SelectedItem as DataRowView;
            Edit(ro, "Models");
        }

        private void EditClass_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView ro = class_list.SelectedItem as DataRowView;
            Edit(ro, "Model_class");
        }

        private void EditEmp_Click_3(object sender, RoutedEventArgs e)
        {
            DataRowView ro = employee_list.SelectedItem as DataRowView;
            Edit(ro, "Employees");
        }

        private void EditOrder_Click_4(object sender, RoutedEventArgs e)
        {
            DataRowView ro = order_list.SelectedItem as DataRowView;
            Edit(ro, "Orders");
        }

        private void EditInvoice_Click_5(object sender, RoutedEventArgs e)
        {
            DataRowView ro = invoice_list.SelectedItem as DataRowView;
            Edit(ro, "Invoices");
        }


        private void EditVio_Click_7(object sender, RoutedEventArgs e)
        {
            DataRowView ro = violation_list.SelectedItem as DataRowView;
            Edit(ro, "Violations");
        }

        private void EditSta_Click_8(object sender, RoutedEventArgs e)
        {
            DataRowView ro = station_list.SelectedItem as DataRowView;
            Edit(ro, "Stations");
        }
        public void Delete(DataRowView ro, string query)
        {
            if (ro != null)
            {
                string id = ro.Row[0].ToString();
                fun.ExecuteQuery(query + $"{id}");
                MessageBox.Show("Informaion has been deleted");
                Update();
            }
            else
                MessageBox.Show("Please choose an object");
        }

        private void DelClient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView ro = client_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Driver_license_info where license_id = ");
        }
        private void DelCar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView ro = car_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Cars where car_id = ");
        }

        private void DelModel_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView ro = model_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Models where model_id = ");
        }

        private void DelClass_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView ro = class_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Model_class where model_class_id = ");
        }

        private void DelEmp_Click_3(object sender, RoutedEventArgs e)
        {
            DataRowView ro = employee_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Employees where employee_id = ");
        }

        private void DelOrder_Click_4(object sender, RoutedEventArgs e)
        {
            DataRowView ro = order_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Orders where order_id = ");
        }


        private void DelVio_Click_6(object sender, RoutedEventArgs e)
        {
            DataRowView ro = violation_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Violations where violation_id = ");
        }

        private void DelTar_Click_7(object sender, RoutedEventArgs e)
        {
            DataRowView ro = tariff_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Tariffs where tariff_id = ");
        }

        private void DelSta_Click_8(object sender, RoutedEventArgs e)
        {
            DataRowView ro = station_list.SelectedItem as DataRowView;
            Delete(ro, $"Delete from Stations where station_id = ");
        }

        private void ExecuteReport_Click(object sender, RoutedEventArgs e)
        {
            string option = optionReport.SelectedItem as string;
            if (option != null)
            {
                string[] q = report_dict[option].Split("/");
                int index = q[0].IndexOf(q[1]);
                if (start_report.SelectedDate != null && end_report.SelectedDate != null)
                {
                    q[0] = q[0].Insert(index-1, $" where {q[2]} >= '{start_report.Text.Replace(".", "-")}' and {q[2]} <= '{end_report.Text.Replace(".", "-")}' ");
                }
                else if (start_report.SelectedDate != null)
                    q[0] = q[0].Insert(index - 1, $" where {q[2]} >= '{start_report.Text.Replace(".", "-")}' ");
                else if (end_report.SelectedDate != null)
                    q[0] = q[0].Insert(index - 1, $" where {q[2]} <= '{end_report.Text.Replace(".", "-")}' ");
               // MessageBox.Show(q[0]);
                report_list.ItemsSource = fun.DataWithAdapt(q[0]).DefaultView;
            }
            else
                MessageBox.Show("Choose an option");
        }

        private void ReportReset_Click(object sender, RoutedEventArgs e)
        {
            start_report.SelectedDate = null;
            end_report.SelectedDate = null;
        }
    }
}
