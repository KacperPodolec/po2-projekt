using Npgsql;
using System;
using System.Collections.Generic;
using EDziennik.Models;
using System.Collections;

public class GradeRepository
{
    private string _connectionString;

    public GradeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Dodawanie nowej oceny
    public void Add(Grade grade)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                INSERT INTO grades (student_id, subject_id, teacher_id, value, weight, grade_date, note)
                VALUES (@studentId, @subjectId, @teacherId, @value, @weight, @gradeDate, @note) RETURNING id", conn);

            cmd.Parameters.AddWithValue("studentId", grade.StudentId);
            cmd.Parameters.AddWithValue("subjectId", grade.SubjectId);
            cmd.Parameters.AddWithValue("teacherId", grade.TeacherId);
            cmd.Parameters.AddWithValue("value", grade.Value);
            cmd.Parameters.AddWithValue("weight", grade.Weight);
            cmd.Parameters.AddWithValue("gradeDate", grade.GradeDate);
            cmd.Parameters.AddWithValue("note", grade.Note ?? "");

            grade.Id = (int)cmd.ExecuteScalar();
        }
    }

    // Edycja oceny
    public void Update(Grade grade)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                UPDATE grades
                SET subject_id=@subjectId, teacher_id=@teacherId, value=@value, weight=@weight, grade_date=@gradeDate, note=@note
                WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("subjectId", grade.SubjectId);
            cmd.Parameters.AddWithValue("teacherId", grade.TeacherId);
            cmd.Parameters.AddWithValue("value", grade.Value);
            cmd.Parameters.AddWithValue("weight", grade.Weight);
            cmd.Parameters.AddWithValue("gradeDate", grade.GradeDate);
            cmd.Parameters.AddWithValue("note", grade.Note ?? "");
            cmd.Parameters.AddWithValue("id", grade.Id);

            // wykonanie UPDATE
            cmd.ExecuteNonQuery();
        }
    }

    // Usuwanie oceny
    public void Delete(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand("DELETE FROM grades WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);

            // wykonanie DELETE
            cmd.ExecuteNonQuery();
        }
    }

    // Metoda pobierająca listę ocen ucznia dla danego przedmiotu
    // Wyświetla wszystkie oceny wybranego ucznia w DateGridView
    public List<GradeListItem> GetByStudentAndSubject(int studentId, int subjectId)
    {
        var list = new List<GradeListItem>();
        string query = "SELECT g.id, g.student_id, g.subject_id, s.name AS subject_name, g.teacher_id, g.value, g.weight, g.grade_date, g.note " +
                       "FROM grades g " +
                       "JOIN subjects s ON g.subject_id = s.id " +
                       "WHERE g.student_id = @studentId AND g.subject_id = @subjectId";

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("studentId", studentId);
                cmd.Parameters.AddWithValue("subjectId", subjectId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new GradeListItem
                        {
                            Id = reader.GetInt32(0),
                            StudentId = reader.GetInt32(1),
                            SubjectId = reader.GetInt32(2),
                            SubjectName = reader.GetString(3),
                            TeacherId = reader.GetInt32(4),
                            Value = reader.GetDecimal(5),
                            Weight = reader.GetDecimal(6),
                            GradeDate = reader.GetDateTime(7),
                            Note = reader.IsDBNull(8) ? "" : reader.GetString(8)
                        });
                    }
                }
            }
        }

        return list; 
    }

    // Metoda pobierająca pojedyńczą ocenę po ID
    // Zwraca pojedyńczą ocene podczas wybrania jej od edycji
    public Grade GetById(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT * FROM grades WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Grade
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            StudentId = reader.GetInt32(reader.GetOrdinal("student_id")),
                            SubjectId = reader.GetInt32(reader.GetOrdinal("subject_id")),
                            TeacherId = reader.GetInt32(reader.GetOrdinal("teacher_id")),
                            Value = reader.GetInt16(reader.GetOrdinal("value")),
                            Weight = reader.GetInt16(reader.GetOrdinal("weight")),
                            GradeDate = reader.GetDateTime(reader.GetOrdinal("grade_date")),
                            Note = reader.IsDBNull(reader.GetOrdinal("note")) ? "" : reader.GetString(reader.GetOrdinal("note"))
                        };
                    }
                }
            }
        }
        return null;
    }
}
