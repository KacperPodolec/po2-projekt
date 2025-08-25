using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class TeacherSubjectClassRepository
    {
        private string _connectionString;

        public TeacherSubjectClassRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TeacherSubjectClass> GetAll()
        {
            var list = new List<TeacherSubjectClass>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, teacher_id, subject_id, class_id FROM teacher_subject_class", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TeacherSubjectClass
                        {
                            Id = reader.GetInt32(0),
                            TeacherId = reader.GetInt32(1),
                            SubjectId = reader.GetInt32(2),
                            ClassId = reader.GetInt32(3)
                        });
                    }
                }
            }
            return list;
        }

        public TeacherSubjectClass GetById(int id)
        {
            TeacherSubjectClass tsc = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, teacher_id, subject_id, class_id FROM teacher_subject_class WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tsc = new TeacherSubjectClass
                            {
                                Id = reader.GetInt32(0),
                                TeacherId = reader.GetInt32(1),
                                SubjectId = reader.GetInt32(2),
                                ClassId = reader.GetInt32(3)
                            };
                        }
                    }
                }
            }
            return tsc;
        }

        public void Add(TeacherSubjectClass tsc)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO teacher_subject_class (teacher_id, subject_id, class_id) VALUES (@teacher_id,@subject_id,@class_id) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("teacher_id", tsc.TeacherId);
                    cmd.Parameters.AddWithValue("subject_id", tsc.SubjectId);
                    cmd.Parameters.AddWithValue("class_id", tsc.ClassId);
                    tsc.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(TeacherSubjectClass tsc)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE teacher_subject_class SET teacher_id=@teacher_id, subject_id=@subject_id, class_id=@class_id WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("teacher_id", tsc.TeacherId);
                    cmd.Parameters.AddWithValue("subject_id", tsc.SubjectId);
                    cmd.Parameters.AddWithValue("class_id", tsc.ClassId);
                    cmd.Parameters.AddWithValue("id", tsc.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM teacher_subject_class WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
