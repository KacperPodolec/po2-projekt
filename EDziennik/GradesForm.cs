using System;
using System.Windows.Forms;
using EDziennik.Models;
using EDziennik.Data.Repositories;
using System.Collections.Generic;


namespace EDziennik
{
    public partial class GradesForm : Form
    {
        private string _connectionString;
        private StudentListItem _selectedStudent;
        private Subject _selectedSubject;
        private GradeListItem _selectedGrade;

        private StudentRepository _studentRepo;
        private SubjectRepository _subjectRepo;
        private GradeRepository _gradeRepo;

        private StudentListItem _selectedStudentItem;
        private List<StudentListItem> _students;

        public GradesForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _gradeRepo = new GradeRepository(_connectionString);
            _subjectRepo = new SubjectRepository(_connectionString);

            dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged;

            LoadStudents();
        }


        private void LoadStudents()
        {
            var repo = new StudentRepository(_connectionString);
            var data = repo.GetAllWithClassNames(); // lista StudentListItem

            dgvStudents.AutoGenerateColumns = true;
            dgvStudents.DataSource = data;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvStudents.Columns["Id"] != null) dgvStudents.Columns["Id"].Visible = false;
            if (dgvStudents.Columns["ClassId"] != null) dgvStudents.Columns["ClassId"].Visible = false;
        }



        private void LoadSubjects()
        {
            cmbSubject.DataSource = _subjectRepo.GetAll();
            cmbSubject.DisplayMember = "Name";
            cmbSubject.ValueMember = "Id";
        }

        private void LoadSubjectsForStudent(int studentId)
        {
            try
            {
                var repo = new SubjectRepository(_connectionString);
                var subjects = repo.GetByStudent(studentId); 
                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "Name";   
                cmbSubject.ValueMember = "Id"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania przedmiotów: {ex.Message}");
            }
        }

        private void LoadSubjectsForSelectedStudent(int studentId)
        {
            var subjects = _subjectRepo.GetByStudent(studentId);

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



        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) return;

            _selectedStudentItem = dgvStudents.CurrentRow.DataBoundItem as StudentListItem;
            if (_selectedStudentItem == null) return;

            lblFirstName.Text = _selectedStudentItem.FirstName;
            lblLastName.Text = _selectedStudentItem.LastName;
            lblBirthDate.Text = _selectedStudentItem.BirthDate.ToShortDateString();
            lblClass.Text = _selectedStudentItem.ClassName;

            LoadSubjectsForSelectedStudent(_selectedStudentItem.Id);
        }



        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedStudentItem == null || cmbSubject.SelectedItem == null) return;

            _selectedSubject = cmbSubject.SelectedItem as Subject;
            LoadGradesForStudentAndSubject(_selectedStudentItem.Id, _selectedSubject.Id);
        }


        private void LoadGrades()
        {
            if (_selectedStudent == null || _selectedSubject == null) return;
            dgvGrades.DataSource = _gradeRepo.GetByStudentAndSubject(_selectedStudent.Id, _selectedSubject.Id);
            dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadGradesForStudentAndSubject(int studentId, int subjectId)
        {
            var grades = _gradeRepo.GetByStudentAndSubject(studentId, subjectId); // lista GradeListItem

            dgvGrades.AutoGenerateColumns = true;
            dgvGrades.DataSource = grades;
            dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ukrycie niepotrzebnych kolumn
            if (dgvGrades.Columns["Id"] != null) dgvGrades.Columns["Id"].Visible = false;
            if (dgvGrades.Columns["StudentId"] != null) dgvGrades.Columns["StudentId"].Visible = false;
            if (dgvGrades.Columns["SubjectId"] != null) dgvGrades.Columns["SubjectId"].Visible = false;
            if (dgvGrades.Columns["TeacherId"] != null) dgvGrades.Columns["TeacherId"].Visible = false;
        }


        private void dgvGrades_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentRow == null) return;
            _selectedGrade = dgvGrades.CurrentRow.DataBoundItem as GradeListItem;

            if (_selectedGrade != null)
            {
                cmbGradeValue.Text = _selectedGrade.Value.ToString();
                cmbGradeWeight.Text = _selectedGrade.Weight.ToString();
                dtpGradeDate.Value = _selectedGrade.GradeDate;
                txtGradeNote.Text = _selectedGrade.Note;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_selectedStudent == null || _selectedSubject == null) return;

            var grade = new Grade
            {
                StudentId = _selectedStudent.Id,
                SubjectId = _selectedSubject.Id,
                TeacherId = 1, // tu możesz wstawić prawdziwego nauczyciela
                Value = decimal.Parse(cmbGradeValue.Text),
                Weight = decimal.Parse(cmbGradeWeight.Text),
                GradeDate = dtpGradeDate.Value,
                Note = txtGradeNote.Text
            };

            _gradeRepo.Add(grade);
            LoadGrades();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedGrade == null) return;

            var grade = new Grade
            {
                Id = _selectedGrade.Id,
                StudentId = _selectedGrade.StudentId,
                SubjectId = _selectedGrade.SubjectId,
                TeacherId = _selectedGrade.TeacherId,
                Value = decimal.Parse(cmbGradeValue.Text),
                Weight = decimal.Parse(cmbGradeWeight.Text),
                GradeDate = dtpGradeDate.Value,
                Note = txtGradeNote.Text
            };

            _gradeRepo.Update(grade);
            LoadGrades();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedGrade == null) return;

            var confirm = MessageBox.Show($"Usunąć ocenę {_selectedGrade.Value}?", "Potwierdzenie", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _gradeRepo.Delete(_selectedGrade.Id);
                LoadGrades();
            }
        }
    }
}
