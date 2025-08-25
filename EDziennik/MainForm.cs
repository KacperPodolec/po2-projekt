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
    public partial class MainForm: Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            string connStr = "Host=localhost;Port=5433;Database=edziennik;Username=postgres;Password=g77ogp;";
            StudentsForm studentsForm = new StudentsForm(connStr);
            studentsForm.ShowDialog();
        }
    }
}
