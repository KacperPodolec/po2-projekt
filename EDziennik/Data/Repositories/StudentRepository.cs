using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class StudentRepository
    {
        private string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, first_name, last_name, birth_date, class_id, created_at FROM students", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            BirthDate = reader.GetDateTime(3),
                            ClassId = reader.GetInt32(4),
                            CreatedAt = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return students;
        }

        public Student GetById(int id)
        {
            Student student = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, first_name, last_name, birth_date, class_id, created_at FROM students WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new Student
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                ClassId = reader.GetInt32(4),
                                CreatedAt = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return student;
        }

        public void Add(Student student)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO students (first_name, last_name, birth_date, class_id) VALUES (@first_name, @last_name, @birth_date, @class_id) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("last_name", student.LastName);
                    cmd.Parameters.AddWithValue("birth_date", student.BirthDate);
                    cmd.Parameters.AddWithValue("class_id", student.ClassId);
                    student.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Student student)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE students SET first_name=@first_name, last_name=@last_name, birth_date=@birth_date, class_id=@class_id WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("last_name", student.LastName);
                    cmd.Parameters.AddWithValue("birth_date", student.BirthDate);
                    cmd.Parameters.AddWithValue("class_id", student.ClassId);
                    cmd.Parameters.AddWithValue("id", student.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM students WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
