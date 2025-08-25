using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    public class ClassRepository
    {
        private string _connectionString;

        public ClassRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Class> GetAll()
        {
            var classes = new List<Class>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name, year, created_at FROM classes", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(new Class
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Year = reader.GetInt16(2),
                            CreatedAt = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return classes;
        }

        public Class GetById(int id)
        {
            Class cls = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name, year, created_at FROM classes WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cls = new Class
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Year = reader.GetInt16(2),
                                CreatedAt = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            return cls;
        }

        public void Add(Class cls)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO classes (name, year) VALUES (@name, @year) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("name", cls.Name);
                    cmd.Parameters.AddWithValue("year", cls.Year);
                    cls.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Class cls)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE classes SET name=@name, year=@year WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("name", cls.Name);
                    cmd.Parameters.AddWithValue("year", cls.Year);
                    cmd.Parameters.AddWithValue("id", cls.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM classes WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
