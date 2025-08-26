# Dokumentacja projektu eDziennik

## Spis treści
1. [Opis projektu](#opis-projektu)
2. [Technologie](#technologie)
3. [Architektura](#architektura)
4. [Modele danych](#modele-danych)
5. [Repozytoria](#repozytoria)
6. [Formularze](#formularze)
7. [Konfiguracja](#konfiguracja)

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

```csharp
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
```

- **Grade**

```csharp
public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public decimal Value { get; set; }
        public decimal Weight { get; set; }
        public DateTime GradeDate { get; set; }
        public string Note { get; set; }
    }
```

- **Subject**

```csharp
public class Subject
    {
        public int Id { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name nie może być pusty");
                _name = value;
            }
        }
        private string _name;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
```

- **Class**

```csharp
public class Class
    {
        public int Id { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name nie może być pusty");
                _name = value;
            }
        }
        private string _name;

        public short Year
        {
            get => _year;
            set
            {
                if (value < 1 || value > 8)
                    throw new ArgumentException("Year musi być 1–8");
                _year = value;
            }
        }
        private short _year;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
```

## Repozytoria
- **StudentRepository** – pobieranie, dodawanie, edycja, usuwanie uczniów.
- **GradeRepository** – operacje na ocenach.
- **SubjectRepository** – zarządzanie przedmiotami.
- **ClassRepository** – zarządzanie klasami.

## Formularze
- **MainForm** – główny formularz aplikacji, z przyciskami do otwierania pozostałych formularzy.
- **StudentsForm** – obsługuje dodawanie, edycję, usuwanie, wyszukiwanie uczniów.
- **GradesForm** – obsługuje dodawanie, edycję i wyświetlanie ocen uczniów w konkretnych przedmiotach.
Każdy formularz posiada metody do walidacji danych, obsługi zdarzeń i odświeżania widoku.

## Konfiguracja
Plik App.config zawiera połączenie z bazą danych:

```csharp
<connectionStrings>
    <add name="EdziennikDb"
         connectionString="Host=localhost;Port=5433;Database=edziennik;Username=postgres;Password=<haslo>"
         providerName="Npgsql" />
  </connectionStrings>
```