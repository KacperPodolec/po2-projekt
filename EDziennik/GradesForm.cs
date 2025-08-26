using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EDziennik.Data.Repositories;
using EDziennik.Models;

namespace EDziennik
{
    public partial class GradesForm : Form
    {
        private string _connectionString;               // Przechowuje connection string do bazy
        private StudentListItem _selectedStudentItem;   // Aktualnie wybrany uczeń w dgvStudents
        private Subject _selectedSubject;               // Aktualnie wybrany przedmiot w cmbSubject
        private GradeListItem _selectedGradeItem;       // Aktualnie wybrana ocena w dgvGrades

        private StudentRepository _studentRepo;         // Repozytorium uczniów
        private SubjectRepository _subjectRepo;         // Repozytorium przedmiotów
        private GradeRepository _gradeRepo;             // Repozytorium ocen

        private int _currentTeacherId;                  // Id aktualnego nauczyciela (ustawione na sztywno na 1)

        public GradesForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;

            // Repozytoria do obsługi bazy danych
            _studentRepo = new StudentRepository(_connectionString);
            _subjectRepo = new SubjectRepository(_connectionString);
            _gradeRepo = new GradeRepository(_connectionString);

            // Zdarzenie kontroli
            dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
            dgvGrades.SelectionChanged += dgvGrades_SelectionChanged;
            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged;

            // Domyślne pierwsze elementry comboBoxów
            if (cmbGradeValue.Items.Count > 0) cmbGradeValue.SelectedIndex = 0;
            if (cmbGradeWeight.Items.Count > 0) cmbGradeWeight.SelectedIndex = 0;

            _currentTeacherId = 1;  // sztywne ID nauczyciela

            LoadStudents();         // załadowanie listy uczniów do DataGridView
        }

        // METODY
        // Ładuje listę uczniów z nazwami klas do DataGridView
        private void LoadStudents()
        {
            var students = _studentRepo.GetAllWithClassNames();
            dgvStudents.AutoGenerateColumns = true;
            dgvStudents.DataSource = students;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ukrywanie kolumn, które mają być niewidoczne w DataGridView
            if (dgvStudents.Columns["Id"] != null) dgvStudents.Columns["Id"].Visible = false;
            if (dgvStudents.Columns["ClassId"] != null) dgvStudents.Columns["ClassId"].Visible = false;
        }

        // Ładuje listę przedmiotów dla wybranego ucznia do ComboBoxa
        private void LoadSubjectsForSelectedStudent()
        {
            if (_selectedStudentItem == null) return;

            var subjects = _subjectRepo.GetByStudent(_selectedStudentItem.Id);
            cmbSubject.DataSource = subjects;
            cmbSubject.DisplayMember = "Name";
            cmbSubject.ValueMember = "Id";

            if (subjects.Count > 0)
            {
                cmbSubject.SelectedIndex = 0;
            }
            else
            {
                dgvGrades.DataSource = null;
            }
        }

        // Aktualizuje wybrany wiersz ucznia i wyświetla jego szczegóły
        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) return;

            _selectedStudentItem = dgvStudents.CurrentRow.DataBoundItem as StudentListItem;
            if (_selectedStudentItem == null) return;

            lblFirstName.Text = _selectedStudentItem.FirstName;
            lblLastName.Text = _selectedStudentItem.LastName;
            lblBirthDate.Text = _selectedStudentItem.BirthDate.ToShortDateString();
            lblClass.Text = _selectedStudentItem.ClassName;

            LoadSubjectsForSelectedStudent();
        }

        // Aktualizuje wybrany przedmiot i ładuje oceny ucznia
        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedStudentItem == null || cmbSubject.SelectedItem == null) return;

            _selectedSubject = cmbSubject.SelectedItem as Subject;
            if (_selectedSubject != null)
            {
                LoadGrades(_selectedStudentItem.Id, _selectedSubject.Id);
            }
        }

        // Ładuje oceny ucznia dla wybranego przedmiotu do DataGridView
        private void LoadGrades(int studentId, int subjectId)
        {
            var grades = _gradeRepo.GetByStudentAndSubject(studentId, subjectId);
            dgvGrades.AutoGenerateColumns = true;
            dgvGrades.DataSource = grades;
            dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

           
            if (dgvGrades.Columns["Id"] != null) dgvGrades.Columns["Id"].Visible = false;
            if (dgvGrades.Columns["StudentId"] != null) dgvGrades.Columns["StudentId"].Visible = false;
            if (dgvGrades.Columns["SubjectId"] != null) dgvGrades.Columns["SubjectId"].Visible = false;
            if (dgvGrades.Columns["TeacherId"] != null) dgvGrades.Columns["TeacherId"].Visible = false;

            _selectedGradeItem = null;
        }

        // Aktualizuje wybraną ocenę w kontrolkach edycyjnych po zmianie zaznaczenia w DataGridView
        private void dgvGrades_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentRow == null) return;

            _selectedGradeItem = dgvGrades.CurrentRow.DataBoundItem as GradeListItem;
            if (_selectedGradeItem == null) return;

            cmbGradeValue.SelectedItem = _selectedGradeItem.Value;
            cmbGradeWeight.SelectedItem = _selectedGradeItem.Weight;
            dtpGradeDate.Value = _selectedGradeItem.GradeDate;
            txtGradeNote.Text = _selectedGradeItem.Note;

            dgvGrades.CurrentRow.Selected = true;
        }

        // Dodaje nową ocenę
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_selectedStudentItem == null || _selectedSubject == null)
            {
                MessageBox.Show("Wybierz ucznia i przedmiot.");
                return;
            }

            var grade = new Grade
            {
                StudentId = _selectedStudentItem.Id,
                SubjectId = _selectedSubject.Id,
                TeacherId = _currentTeacherId,
                Value = Convert.ToInt32(cmbGradeValue.SelectedItem),
                Weight = Convert.ToInt32(cmbGradeWeight.SelectedItem),
                GradeDate = dtpGradeDate.Value,
                Note = txtGradeNote.Text
            };

            _gradeRepo.Add(grade);
            LoadGrades(_selectedStudentItem.Id, _selectedSubject.Id);
        }

        // Edytuje ocenę w bazie
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedGradeItem == null)
            {
                MessageBox.Show("Wybierz ocenę do edycji.");
                return;
            }

            var grade = _gradeRepo.GetById(_selectedGradeItem.Id);
            if (grade == null) return;

            grade.Value = Convert.ToInt32(cmbGradeValue.SelectedItem);
            grade.Weight = Convert.ToInt32(cmbGradeWeight.SelectedItem);
            grade.GradeDate = dtpGradeDate.Value;
            grade.Note = txtGradeNote.Text;

            _gradeRepo.Update(grade);
            LoadGrades(_selectedStudentItem.Id, _selectedSubject.Id);
        }

        // Usuwa ocenę z bazy
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedGradeItem == null)
            {
                MessageBox.Show("Wybierz ocenę do usunięcia.");
                return;
            }

            var confirm = MessageBox.Show("Na pewno chcesz usunąć tę ocenę?", "Potwierdzenie", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            _gradeRepo.Delete(_selectedGradeItem.Id);
            LoadGrades(_selectedStudentItem.Id, _selectedSubject.Id);
        }
    }
}
