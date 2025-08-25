using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using EDziennik.Data;

namespace EDziennik
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["EdziennikDb"].ConnectionString;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM students;";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        int studentCount = Convert.ToInt32(cmd.ExecuteScalar());
                        MessageBox.Show($"Liczba uczniów w tabeli students: {studentCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia / odczytu danych: {ex.Message}");
            }
        }
    }
}
