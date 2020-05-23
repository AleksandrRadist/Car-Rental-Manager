using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace car_rental
{
    public class Repository
    {
        public DataTable ConvertToDate(List<string> cel, DataTable table1)
        {
            DataTable table2 = table1.Copy();
            foreach (string j in cel)
            {
                table2.Columns.Remove(j);
                table2.Columns.Add(j, typeof(string));
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    table2.Rows[i][j] = table1.Rows[i].Field<DateTime>(j).ToString("MM-dd-yyyy");
                }
            }
            return table2;
        }
    }
}
