using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace car_rental
{
    public class DB_Access
    {
        private static SqlConnection con = new SqlConnection();
        private static SqlCommand com = new SqlCommand();
        private static SqlDataAdapter adapter = new SqlDataAdapter();
        public SqlTransaction tran;

        private static string conStr = @"Data Source=asus\mssqlserver01;Initial Catalog=car_rental_4;Integrated Security=True";

        public void CreateCon()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.ConnectionString = conStr;
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CloseCon()
        {
            con.Close();
        }
        public DataTable DataWithAdapt(string query) 
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    CreateCon();
                }
                DataTable tbl = new DataTable();
                com.Connection = con;
     
                com.CommandText = query;
                com.CommandType = CommandType.Text;

                adapter = new SqlDataAdapter(com);
                adapter.Fill(tbl);
                return tbl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecuteQuery(string query)
        { 
            try
            {
                if (con.State == 0)
                {
                    CreateCon();
                }
                
                com.Connection = con;
                com.CommandText = query;
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        }
    }


