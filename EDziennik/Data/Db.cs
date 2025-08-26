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
    // Statyczna klasa Db odpowiedzialna za tworzenie połączeń do bazy danych
    public static class Db
    {
        // Tworzy i zwraca nowe połączenie do bazy
        public static NpgsqlConnection GetConnection()
        {
            string connString = ConfigurationManager
                .ConnectionStrings["EdziennikDb"].ConnectionString;
            MessageBox.Show($"Connection string: {connString}");

            return new NpgsqlConnection(connString);
        }
    }
}
