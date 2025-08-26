# Dokumentacja projektu eDziennik

## Spis treści
1. [Opis projektu](#opis-projektu)
2. [Technologie](#technologie)
3. [Architektura](#architektura)
4. [Modele danych](#modele-danych)
5. [Repozytoria](#repozytoria)
6. [Formularze](#formularze)
7. [Diagram ERD](#diagram-erd)
8. [Konfiguracja](#konfiguracja)

## Opis projektu
EDziennik to aplikacja desktopowa napisana w C# z wykorzystaniem Windows Forms i bazy PostgreSQL.
Celem projektu jest umożliwienie prowadzenia dziennika szkolnego z zarządzaniem uczniami, klasami, przedmiotami i ocenami.

## Technologie
- .NET Framework 4.8
- C#
- WindowsForms (WinForms)
- PostgreSQL (baza danych)
- Npgsl (provider PostgreSQL dla .NET)

## Architektura
1. **Modele** (Models) - klasy odpowiadajace tabelom w bazie danych (Student, Grade, Subject, Class itd.).
2. **Repozytoria** (Data/Repositories) - klasy obsługujące dostęp do danych i operacje CRUD.
3. **Formularze** (Forms) - UI dla użytkownika (MainForm, StudentsForm, GradesForm).

## Modele danych
Przykładowe modele w projekcie:
- **Student**

'''cs
public class Student
    {
        public int Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FirstName nie może być pusty");
                _firstName = value;
            }
        }
        private string _firstName;

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("LastName nie może być pusty");
                _lastName = value;
            }
        }
        private string _lastName;

        public DateTime BirthDate { get; set; }
        public int ClassId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ClassName { get; set; }

    }
'''
