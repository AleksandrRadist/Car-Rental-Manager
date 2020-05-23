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

namespace car_rental_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        Repository rep = new Repository();
        public MainMenu()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {
            clientList.ItemsSource = rep.Clients;
        } 
    }
}
