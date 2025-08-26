using System;
using System.Collections.Generic;
using Npgsql;
using EDziennik.Models;

namespace EDziennik.Data.Repositories
{
    // Repozytorium odpowiedzialne za obsługę uczniów w bazie danych.
    public class StudentRepository
    {
        private string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        } 

        // Metoda pobiera pojedyńczego ucznia po jego ID
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

        // Metoda pobiera wszystkich uczniów wraz z nazwami klas, do których należą
        public List<StudentListItem> GetAllWithClassNames()
        {
            var students = new List<StudentListItem>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    @"SELECT s.id, s.first_name, s.last_name, s.birth_date, s.class_id, c.name AS class_name
              FROM students s
              JOIN classes c ON s.class_id = c.id", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new StudentListItem
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            BirthDate = reader.GetDateTime(3),
                            ClassId = reader.GetInt32(4),
                            ClassName = reader.GetString(5)
                        });
                    }
                }
            }
            return students;
        }

        // Metoda dodająca nowego ucznia
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

        // Metoda aktualizująca dane ucznia
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

        // Metoda usuwająca dane ucznia
        public void Delete(int studentId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM Students WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", studentId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Nie znaleziono ucznia do usunięcia.");
                    }
                }
            }
        }

        // Metoda wyszukująca uczniów po imieniu i nazwisku
        public List<StudentListItem> SearchByName(string keyword)
        {
            var students = new List<StudentListItem>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    @"SELECT s.id, s.first_name, s.last_name, s.birth_date, c.name as class_name
              FROM students s
              LEFT JOIN classes c ON s.class_id = c.id
              WHERE s.first_name ILIKE @kw OR s.last_name ILIKE @kw", conn))
                {
                    cmd.Parameters.AddWithValue("kw", $"%{keyword}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentListItem
                            {
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                ClassName = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return students;
        }
    }
}
