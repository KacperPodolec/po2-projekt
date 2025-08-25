using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class GradeRepository
    {
        private string _connectionString;

        public GradeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Grade> GetAll()
        {
            var grades = new List<Grade>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, student_id, subject_id, teacher_id, value, weight, grade_date, note, created_at FROM grades", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        grades.Add(new Grade
                        {
                            Id = reader.GetInt32(0),
                            StudentId = reader.GetInt32(1),
                            SubjectId = reader.GetInt32(2),
                            TeacherId = reader.GetInt32(3),
                            Value = reader.GetInt16(4),
                            Weight = reader.GetInt16(5),
                            GradeDate = reader.GetDateTime(6),
                            Note = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            CreatedAt = reader.GetDateTime(8)
                        });
                    }
                }
            }
            return grades;
        }

        public Grade GetById(int id)
        {
            Grade grade = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, student_id, subject_id, teacher_id, value, weight, grade_date, note, created_at FROM grades WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            grade = new Grade
                            {
                                Id = reader.GetInt32(0),
                                StudentId = reader.GetInt32(1),
                                SubjectId = reader.GetInt32(2),
                                TeacherId = reader.GetInt32(3),
                                Value = reader.GetInt16(4),
                                Weight = reader.GetInt16(5),
                                GradeDate = reader.GetDateTime(6),
                                Note = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                CreatedAt = reader.GetDateTime(8)
                            };
                        }
                    }
                }
            }
            return grade;
        }

        public void Add(Grade grade)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO grades (student_id, subject_id, teacher_id, value, weight, grade_date, note) " +
                    "VALUES (@student_id, @subject_id, @teacher_id, @value, @weight, @grade_date, @note) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("student_id", grade.StudentId);
                    cmd.Parameters.AddWithValue("subject_id", grade.SubjectId);
                    cmd.Parameters.AddWithValue("teacher_id", grade.TeacherId);
                    cmd.Parameters.AddWithValue("value", grade.Value);
                    cmd.Parameters.AddWithValue("weight", grade.Weight);
                    cmd.Parameters.AddWithValue("grade_date", grade.GradeDate);
                    cmd.Parameters.AddWithValue("note", string.IsNullOrEmpty(grade.Note) ? (object)DBNull.Value : grade.Note);
                    grade.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Grade grade)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE grades SET student_id=@student_id, subject_id=@subject_id, teacher_id=@teacher_id, value=@value, weight=@weight, grade_date=@grade_date, note=@note WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("student_id", grade.StudentId);
                    cmd.Parameters.AddWithValue("subject_id", grade.SubjectId);
                    cmd.Parameters.AddWithValue("teacher_id", grade.TeacherId);
                    cmd.Parameters.AddWithValue("value", grade.Value);
                    cmd.Parameters.AddWithValue("weight", grade.Weight);
                    cmd.Parameters.AddWithValue("grade_date", grade.GradeDate);
                    cmd.Parameters.AddWithValue("note", string.IsNullOrEmpty(grade.Note) ? (object)DBNull.Value : grade.Note);
                    cmd.Parameters.AddWithValue("id", grade.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM grades WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
