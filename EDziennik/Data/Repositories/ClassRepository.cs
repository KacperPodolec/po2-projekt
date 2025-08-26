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
    }
}
