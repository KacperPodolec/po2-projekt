using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EDziennik.Data.Repositories;
using EDziennik.Models;

namespace EDziennik
{
    public partial class StudentsForm : Form
    {
        private string _connectionString;
        private StudentListItem _selectedStudentItem;

        public StudentsForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            LoadClasses();  // ładuje listę klas do comboboxa
            LoadStudents(); // ładuje uczniów do DataGridView
        }

        // Ładuje wszystkie klasy z bazy danych do comboboxa
        private void LoadClasses()
        {
            var classRepo = new ClassRepository(_connectionString);
            cmbClass.DataSource = classRepo.GetAll();
            cmbClass.DisplayMember = "Name";
            cmbClass.ValueMember = "Id";
            cmbClass.SelectedIndex = -1; // brak domyślnego wyboru
        }

        // Ładuje wszystkich uczniów z bazy danych do DataGridView
        private void LoadStudents()
        {
            try
            {
                var repo = new StudentRepository(_connectionString);
                var data = repo.GetAllWithClassNames();

                dgvStudents.AutoGenerateColumns = true;
                dgvStudents.DataSource = data;
                dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // ukrywanie niepotrzebnych kolumn
                if (dgvStudents.Columns["Id"] != null)
                    dgvStudents.Columns["Id"].Visible = false;
                if (dgvStudents.Columns["ClassId"] != null)
                    dgvStudents.Columns["ClassId"].Visible = false;
                if (dgvStudents.Columns["CreatedAt"] != null)
                    dgvStudents.Columns["CreatedAt"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania uczniów: {ex.Message}");
            }
        }

        // Waliduje pola formularza przed dodaniem lub edycją ucznia
        private bool ValidateStudentFields()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Imię nie może być puste.");
                return false;
            }

            if (!txtFirstName.Text.All(Char.IsLetter))
            {
                MessageBox.Show("Imię może zawierać tylko litery.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Nazwisko nie może być puste.");
                return false;
            }

            if (!txtLastName.Text.All(Char.IsLetter))
            {
                MessageBox.Show("Nazwisko może zawierać tylko litery.");
                return false;
            }

            if (cmbClass.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz klasę.");
                return false;
            }

            return true;
        }

        // Dodaje nowego ucznia do bazy danych
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateStudentFields())
                return;

            var student = new Student
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                BirthDate = dtpBirthDate.Value,
                ClassId = (int)cmbClass.SelectedValue
            };

            try
            {
                var repo = new StudentRepository(_connectionString);
                repo.Add(student);
                MessageBox.Show("Uczeń został dodany.");
                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania ucznia: {ex.Message}");
            }
        }

        // Edytuje dane wybranego ucznia w bazie danych
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedStudentItem == null)
            {
                MessageBox.Show("Wybierz ucznia.");
                return;
            }

            if (!ValidateStudentFields())
                return;

            try
            {
                var repo = new StudentRepository(_connectionString);
                var student = repo.GetById(_selectedStudentItem.Id);

                if (student == null)
                {
                    MessageBox.Show("Nie można odnaleźć ucznia w bazie.");
                    return;
                }

                student.FirstName = txtFirstName.Text;
                student.LastName = txtLastName.Text;
                student.BirthDate = dtpBirthDate.Value;
                student.ClassId = (int)cmbClass.SelectedValue;

                repo.Update(student);
                MessageBox.Show("Uczeń został zaktualizowany.");
                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas edycji ucznia: {ex.Message}");
            }
        }

        // Usuwa wybranego ucznia z bazy danych po potwierdzeniu
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudentItem == null)
            {
                MessageBox.Show("Wybierz ucznia.");
                return;
            }

            var result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć {_selectedStudentItem.FirstName} {_selectedStudentItem.LastName}?",
                "Potwierdzenie",
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    var repo = new StudentRepository(_connectionString);
                    repo.Delete(_selectedStudentItem.Id);
                    MessageBox.Show("Uczeń został usunięty.");
                    LoadStudents();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas usuwania ucznia: {ex.Message}");
                }
            }
        }

        // Aktualizuje formularz, gdy zmieni się zaznaczenie w DataGridView
        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null)
            {
                // Konwersja wiersza na obiekt powiązany z DataSource
                _selectedStudentItem = dgvStudents.CurrentRow.DataBoundItem as StudentListItem;

                if (_selectedStudentItem != null)
                {
                    txtFirstName.Text = _selectedStudentItem.FirstName;
                    txtLastName.Text = _selectedStudentItem.LastName;
                    dtpBirthDate.Value = _selectedStudentItem.BirthDate;
                    cmbClass.SelectedValue = _selectedStudentItem.ClassId;
                }
            }
        }

        // Wyszukuje uczniów po imieniu lub nazwisku i aktualizuje DataGridView
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = new StudentRepository(_connectionString);
                string query = txtSearch.Text.Trim();

                List<StudentListItem> results;

                if (string.IsNullOrWhiteSpace(query))
                    results = repo.GetAllWithClassNames();
                else
                    results = repo.SearchByName(query); // metoda w repozytorium, zwraca List<StudentListItem>

                dgvStudents.DataSource = null;
                dgvStudents.DataSource = results;
                dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // ukrywanie kolumn
                if (dgvStudents.Columns["Id"] != null)
                    dgvStudents.Columns["Id"].Visible = false;
                if (dgvStudents.Columns["ClassId"] != null)
                    dgvStudents.Columns["ClassId"].Visible = false;
                if (dgvStudents.Columns["CreatedAt"] != null)
                    dgvStudents.Columns["CreatedAt"].Visible = false;

                _selectedStudentItem = null;
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wyszukiwania uczniów: {ex.Message}");
            }
        }

        // Czyści pola formularza i odznacza zaznaczonego ucznia
        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpBirthDate.Value = DateTime.Today;
            cmbClass.SelectedIndex = -1;
            _selectedStudentItem = null;
        }
    }
}
