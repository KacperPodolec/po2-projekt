using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class AttendanceRepository
    {
        private string _connectionString;

        public AttendanceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Attendance> GetAll()
        {
            var list = new List<Attendance>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, student_id, subject_id, teacher_id, lesson_date, status, note, created_at FROM attendance", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Attendance
                        {
                            Id = reader.GetInt32(0),
                            StudentId = reader.GetInt32(1),
                            SubjectId = reader.GetInt32(2),
                            TeacherId = reader.GetInt32(3),
                            LessonDate = reader.GetDateTime(4),
                            Status = reader.GetString(5)[0],
                            Note = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            CreatedAt = reader.GetDateTime(7)
                        });
                    }
                }
            }
            return list;
        }

        public Attendance GetById(int id)
        {
            Attendance att = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, student_id, subject_id, teacher_id, lesson_date, status, note, created_at FROM attendance WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            att = new Attendance
                            {
                                Id = reader.GetInt32(0),
                                StudentId = reader.GetInt32(1),
                                SubjectId = reader.GetInt32(2),
                                TeacherId = reader.GetInt32(3),
                                LessonDate = reader.GetDateTime(4),
                                Status = reader.GetString(5)[0],
                                Note = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                CreatedAt = reader.GetDateTime(7)
                            };
                        }
                    }
                }
            }
            return att;
        }

        public void Add(Attendance att)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO attendance (student_id, subject_id, teacher_id, lesson_date, status, note) VALUES (@student_id,@subject_id,@teacher_id,@lesson_date,@status,@note) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("student_id", att.StudentId);
                    cmd.Parameters.AddWithValue("subject_id", att.SubjectId);
                    cmd.Parameters.AddWithValue("teacher_id", att.TeacherId);
                    cmd.Parameters.AddWithValue("lesson_date", att.LessonDate);
                    cmd.Parameters.AddWithValue("status", att.Status.ToString());
                    cmd.Parameters.AddWithValue("note", string.IsNullOrEmpty(att.Note) ? (object)DBNull.Value : att.Note);
                    att.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Attendance att)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE attendance SET student_id=@student_id, subject_id=@subject_id, teacher_id=@teacher_id, lesson_date=@lesson_date, status=@status, note=@note WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("student_id", att.StudentId);
                    cmd.Parameters.AddWithValue("subject_id", att.SubjectId);
                    cmd.Parameters.AddWithValue("teacher_id", att.TeacherId);
                    cmd.Parameters.AddWithValue("lesson_date", att.LessonDate);
                    cmd.Parameters.AddWithValue("status", att.Status.ToString());
                    cmd.Parameters.AddWithValue("note", string.IsNullOrEmpty(att.Note) ? (object)DBNull.Value : att.Note);
                    cmd.Parameters.AddWithValue("id", att.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM attendance WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
