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

        // Pobiera wszystkie przedmioty
        public List<Subject> GetAll()
        {
            var subjects = new List<Subject>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name FROM subjects", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return subjects;
        }

        // Pobiera przedmioty dla danego ucznia
        public List<Subject> GetByStudent(int studentId)
        {
            var subjects = new List<Subject>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    @"SELECT DISTINCT s.id, s.name 
                      FROM subjects s
                      JOIN grades g ON g.subject_id = s.id
                      WHERE g.student_id = @studentId", conn))
                {
                    cmd.Parameters.AddWithValue("studentId", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return subjects;
        }
    }
}
