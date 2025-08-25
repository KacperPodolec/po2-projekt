using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class TeacherRepository
    {
        private string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Teacher> GetAll()
        {
            var teachers = new List<Teacher>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, first_name, last_name, user_id, created_at FROM teachers", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            UserId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            CreatedAt = reader.GetDateTime(4)
                        });
                    }
                }
            }
            return teachers;
        }

        public Teacher GetById(int id)
        {
            Teacher teacher = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, first_name, last_name, user_id, created_at FROM teachers WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            teacher = new Teacher
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                CreatedAt = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }
            return teacher;
        }

        public void Add(Teacher teacher)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO teachers (first_name, last_name, user_id) VALUES (@first_name, @last_name, @user_id) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("first_name", teacher.FirstName);
                    cmd.Parameters.AddWithValue("last_name", teacher.LastName);
                    cmd.Parameters.AddWithValue("user_id", teacher.UserId == 0 ? (object)DBNull.Value : teacher.UserId);
                    teacher.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Teacher teacher)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE teachers SET first_name=@first_name, last_name=@last_name, user_id=@user_id WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("first_name", teacher.FirstName);
                    cmd.Parameters.AddWithValue("last_name", teacher.LastName);
                    cmd.Parameters.AddWithValue("user_id", teacher.UserId == 0 ? (object)DBNull.Value : teacher.UserId);
                    cmd.Parameters.AddWithValue("id", teacher.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM teachers WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
