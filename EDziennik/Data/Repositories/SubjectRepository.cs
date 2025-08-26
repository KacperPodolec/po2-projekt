using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    // Repozytorium odpowiedzialne za obsługę przedmiotów
    public class SubjectRepository
    {
        private string _connectionString;

        public SubjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Pobiera listę przedmiotów, z których uczeń ma wystawione oceny.
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
