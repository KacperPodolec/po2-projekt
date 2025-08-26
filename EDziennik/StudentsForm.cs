using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDziennik.Data.Repositories;
using EDziennik.Models;

namespace EDziennik
{
    public partial class StudentsForm: Form
    {
        private readonly string _connectionString;

        public StudentsForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;

            this.Load += StudentsForm_Load;
            this.dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            LoadClasses();
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                var repo = new StudentRepository(_connectionString);
                var data = repo.GetAllWithClassNames();

                dgvStudents.AutoGenerateColumns = true;
                dgvStudents.DataSource = data;
                dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvStudents.Columns["Id"] != null)
                    dgvStudents.Columns["Id"].Visible = false;
                if (dgvStudents.Columns["ClassId"] != null)
                    dgvStudents.Columns["ClassId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania uczniów: {ex.Message}");
            }
        }

        private void LoadClasses()
        {
            var classRepo = new ClassRepository(_connectionString);
            var classes = classRepo.GetAll();
            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "Name";
            cmbClass.ValueMember = "Id";
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = new StudentRepository(_connectionString);

                var student = new Student
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    BirthDate = dtpBirthDate.Value,
                    ClassId = (int)cmbClass.SelectedValue
                };

                repo.Add(student);
                LoadStudents();

                MessageBox.Show("Uczeń został dodany.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania: {ex.Message}");
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Wybierz ucznia do edycji.");
                return;
            }

            try
            {
                var repo = new StudentRepository(_connectionString);

                var student = new Student
                {
                    Id = (int)dgvStudents.CurrentRow.Cells["Id"].Value,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    BirthDate = dtpBirthDate.Value,
                    ClassId = (int)cmbClass.SelectedValue
                };

                repo.Update(student);
                LoadStudents();

                MessageBox.Show("Dane ucznia zostały zaktualizowane.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas edycji: {ex.Message}");
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Wybierz ucznia do usunięcia.");
                return;
            }

            var confirm = MessageBox.Show("Czy na pewno chcesz usunąć ucznia?", "Potwierdzenie", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.No) return;

            try
            {
                var repo = new StudentRepository(_connectionString);

                int id = (int)dgvStudents.CurrentRow.Cells["Id"].Value;
                repo.Delete(id);
                LoadStudents();

                MessageBox.Show("Uczeń został usunięty.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas usuwania: {ex.Message}");
            }
        }


        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null)
            {
                txtFirstName.Text = dgvStudents.CurrentRow.Cells["FirstName"].Value.ToString();
                txtLastName.Text = dgvStudents.CurrentRow.Cells["LastName"].Value.ToString();
                dtpBirthDate.Value = (DateTime)dgvStudents.CurrentRow.Cells["BirthDate"].Value;
                cmbClass.SelectedValue = dgvStudents.CurrentRow.Cells["ClassId"].Value;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtSearch.Text.Trim();
                var repo = new StudentRepository(_connectionString);

                if (string.IsNullOrEmpty(keyword))
                {
                    // Jeśli pole jest puste, ładujemy wszystkich uczniów
                    dgvStudents.DataSource = repo.GetAll();
                }
                else
                {
                    dgvStudents.DataSource = repo.Search(keyword);
                }

                dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wyszukiwania: {ex.Message}");
            }
        }
    }
}
