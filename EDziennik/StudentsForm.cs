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
                var student = new Student
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = lblLastName.Text.Trim(),
                    BirthDate = DateTime.Parse(lblBirthDate.Text.Trim()),
                    ClassId = (int)cmbClass.SelectedValue
                };

                var repo = new StudentRepository(_connectionString);
                repo.Add(student);

                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd dodawania ucznia: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) return;

            try
            {
                var id = (int)dgvStudents.CurrentRow.Cells["Id"].Value;

                var student = new Student
                {
                    Id = id,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = lblLastName.Text.Trim(),
                    BirthDate = DateTime.Parse(lblBirthDate.Text.Trim()),
                    ClassId = (int)cmbClass.SelectedValue
                };

                var repo = new StudentRepository(_connectionString);
                repo.Update(student);

                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd edycji ucznia: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) return;

            var id = (int)dgvStudents.CurrentRow.Cells["Id"].Value;
            var confirm = MessageBox.Show("Czy na pewno chcesz usunąć ucznia?",
                                          "Potwierdzenie", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                var repo = new StudentRepository(_connectionString);
                repo.Delete(id);
                LoadStudents();
            }
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            var item = dgvStudents.CurrentRow?.DataBoundItem as StudentListItem;
            if (item == null) return;

            txtFirstName.Text = item.FirstName ?? string.Empty;
            txtLastName.Text = item.LastName ?? string.Empty;
            txtBirthDate.Text = item.BirthDate.ToString("yyyy-MM-dd");
            cmbClass.SelectedValue = item.ClassId;
        }

    }
}
