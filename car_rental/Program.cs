using System;
using System.Data;
using System.Data.SqlClient;

namespace car_rental
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date1 = DateTime.Parse("27.05.2020");
            DateTime date2 = DateTime.Parse("5.6.2020 11:00:00");
            TimeSpan len = (date2) - (date1);
            int days = len.Days;
            Console.WriteLine(days);
        }
    }
}
