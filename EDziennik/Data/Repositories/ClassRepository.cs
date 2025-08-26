using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    // Repozytorium odpowiedzialne za dostęp do tabeli "classes"
    public class ClassRepository
    {
        private string _connectionString; // prywatne pole przechowujące connection string do bazy

        public ClassRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Metoda zwraca listę wszystkich klas z tabeli "classes"
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
                        // Dodawanie obiektu "Class" do listy
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
