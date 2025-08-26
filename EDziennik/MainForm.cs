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
    // Główny formularz aplikacji dziennika
    public partial class MainForm: Form
    {
        // Inicjalizuje komponenty formularza
        public MainForm()
        {
            InitializeComponent();
        }

        // Otwiera okno zarządzania uczniami
        private void btnStudents_Click(object sender, EventArgs e)
        {
            // Tworzenie połączenia z bazą danych
            string connStr = "Host=localhost;Port=5433;Database=edziennik;Username=postgres;Password=g77ogp;";
            StudentsForm studentsForm = new StudentsForm(connStr);
            studentsForm.ShowDialog();
        }

        // Otwiera okno zarządzania ocenami
        private void btnGrades_Click(object sender, EventArgs e)
        {
            // Tworzenie połączenia z bazą danych
            string connStr = "Host=localhost;Port=5433;Database=edziennik;Username=postgres;Password=g77ogp;";
            GradesForm gradesForm = new GradesForm(connStr);
            gradesForm.ShowDialog();
        }
    }
}
