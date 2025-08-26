DROP TABLE IF EXISTS attendance CASCADE;
DROP TABLE IF EXISTS grades CASCADE;
DROP TABLE IF EXISTS teacher_subject_class CASCADE;
DROP TABLE IF EXISTS students CASCADE;
DROP TABLE IF EXISTS classes CASCADE;
DROP TABLE IF EXISTS subjects CASCADE;
DROP TABLE IF EXISTS teachers CASCADE;
DROP TABLE IF EXISTS users CASCADE;


-- Użytkownicy (proste logowanie dla nauczycieli/admina)
CREATE TABLE users (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
username TEXT NOT NULL UNIQUE,
password_hash TEXT NOT NULL,
role TEXT NOT NULL CHECK (role IN ('admin','teacher')),
created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);


-- Nauczyciele
CREATE TABLE teachers (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
first_name TEXT NOT NULL,
last_name TEXT NOT NULL,
user_id INTEGER UNIQUE,
created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
CONSTRAINT fk_teacher_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL
);


-- Klasy (oddziały)
CREATE TABLE classes (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
name TEXT NOT NULL UNIQUE, -- np. '1A'
year SMALLINT NOT NULL CHECK (year BETWEEN 1 AND 8),
created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);


-- Uczniowie
CREATE TABLE students (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
first_name TEXT NOT NULL,
last_name TEXT NOT NULL,
birth_date DATE NOT NULL,
class_id INTEGER NOT NULL,
created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
CONSTRAINT fk_student_class FOREIGN KEY (class_id) REFERENCES classes(id) ON DELETE RESTRICT
);
CREATE INDEX idx_students_class ON students(class_id);

-- Przedmioty
CREATE TABLE subjects (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
name TEXT NOT NULL UNIQUE,
created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);


-- Kto uczy (nauczyciel) jakiego przedmiotu w jakiej klasie
CREATE TABLE teacher_subject_class (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
teacher_id INTEGER NOT NULL,
subject_id INTEGER NOT NULL,
class_id INTEGER NOT NULL,
UNIQUE (teacher_id, subject_id, class_id),
CONSTRAINT fk_tsc_teacher FOREIGN KEY (teacher_id) REFERENCES teachers(id) ON DELETE CASCADE,
CONSTRAINT fk_tsc_subject FOREIGN KEY (subject_id) REFERENCES subjects(id) ON DELETE CASCADE,
CONSTRAINT fk_tsc_class FOREIGN KEY (class_id) REFERENCES classes(id) ON DELETE CASCADE
);
CREATE INDEX idx_tsc_class ON teacher_subject_class(class_id);

-- Oceny (1–6; waga 1–5)
CREATE TABLE grades (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
student_id INTEGER NOT NULL,
subject_id INTEGER NOT NULL,
teacher_id INTEGER NOT NULL,
value SMALLINT NOT NULL CHECK (value BETWEEN 1 AND 6),
weight SMALLINT NOT NULL CHECK (weight BETWEEN 1 AND 5),
grade_date DATE NOT NULL DEFAULT CURRENT_DATE,
note TEXT,
created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
CONSTRAINT fk_grade_student FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE,
CONSTRAINT fk_grade_subject FOREIGN KEY (subject_id) REFERENCES subjects(id) ON DELETE RESTRICT,
CONSTRAINT fk_grade_teacher FOREIGN KEY (teacher_id) REFERENCES teachers(id) ON DELETE RESTRICT
);
CREATE INDEX idx_grades_student ON grades(student_id);
CREATE INDEX idx_grades_subject ON grades(subject_id);


-- Obecności
CREATE TABLE attendance (
id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
student_id INTEGER NOT NULL,
subject_id INTEGER NOT NULL,
teacher_id INTEGER NOT NULL,
lesson_date DATE NOT NULL,
status CHAR(1) NOT NULL CHECK (status IN ('P','A','E','L')), -- P obecny, A nieob., E uspraw., L spóźn.
note TEXT,
created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
CONSTRAINT fk_att_student FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE,
CONSTRAINT fk_att_subject FOREIGN KEY (subject_id) REFERENCES subjects(id) ON DELETE RESTRICT,
CONSTRAINT fk_att_teacher FOREIGN KEY (teacher_id) REFERENCES teachers(id) ON DELETE RESTRICT,
CONSTRAINT uq_att UNIQUE(student_id, subject_id, lesson_date)
);
CREATE INDEX idx_att_student ON attendance(student_id);
CREATE INDEX idx_att_subject ON attendance(subject_id);