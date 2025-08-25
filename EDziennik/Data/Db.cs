using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using System.Windows.Forms;

namespace EDziennik.Data
{
    public static class Db
    {
        public static NpgsqlConnection GetConnection()
        {
            string connString = ConfigurationManager
                .ConnectionStrings["EdziennikDb"].ConnectionString;
            MessageBox.Show($"Connection string: {connString}");

            return new NpgsqlConnection(connString);
        }
    }
}
