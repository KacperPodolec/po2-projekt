using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class SubjectRepository
    {
        private string _connectionString;

        public SubjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Subject> GetAll()
        {
            var subjects = new List<Subject>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name, created_at FROM subjects", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            CreatedAt = reader.GetDateTime(2)
                        });
                    }
                }
            }
            return subjects;
        }

        public Subject GetById(int id)
        {
            Subject subject = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name, created_at FROM subjects WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subject = new Subject
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2)
                            };
                        }
                    }
                }
            }
            return subject;
        }

        public void Add(Subject subject)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO subjects (name) VALUES (@name) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("name", subject.Name);
                    subject.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Subject subject)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("UPDATE subjects SET name=@name WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("name", subject.Name);
                    cmd.Parameters.AddWithValue("id", subject.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM subjects WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
