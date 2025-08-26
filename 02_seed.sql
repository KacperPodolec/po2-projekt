-- Użytkownicy i nauczyciele
INSERT INTO users (username, password_hash, role) VALUES
('admin', 'PLAIN:admin123', 'admin'), -- w MVP można trzymać hasła w formie jawnej/placeholder (na zajęciach)
('anowak', 'PLAIN:nowak123', 'teacher'),
('jkowalski','PLAIN:kowal123','teacher');


INSERT INTO teachers (first_name, last_name, user_id) VALUES
('Anna','Nowak', (SELECT id FROM users WHERE username='anowak')),
('Jan', 'Kowalski', (SELECT id FROM users WHERE username='jkowalski'));


-- Klasy
INSERT INTO classes (name, year) VALUES ('1A',1), ('1B',1);


-- Uczniowie
INSERT INTO students (first_name, last_name, birth_date, class_id) VALUES
('Karol','Wiśniewski','2013-04-12',(SELECT id FROM classes WHERE name='1A')),
('Zofia','Kamińska','2013-08-02',(SELECT id FROM classes WHERE name='1A')),
('Maja','Zielińska','2013-12-01',(SELECT id FROM classes WHERE name='1B')),
('Tomasz','Wójcik','2013-02-23',(SELECT id FROM classes WHERE name='1B'));

-- Przedmioty
INSERT INTO subjects (name) VALUES ('Matematyka'), ('Język polski'), ('Informatyka');


-- Mapowanie nauczyciel-przedmiot-klasa
INSERT INTO teacher_subject_class (teacher_id, subject_id, class_id)
SELECT t.id, s.id, c.id
FROM teachers t, subjects s, classes c
WHERE (t.last_name='Nowak' AND s.name IN ('Matematyka','Informatyka') AND c.name='1A')
OR (t.last_name='Kowalski' AND s.name IN ('Język polski') AND c.name='1A')
OR (t.last_name='Kowalski' AND s.name IN ('Matematyka') AND c.name='1B');


-- Oceny
INSERT INTO grades (student_id, subject_id, teacher_id, value, weight, grade_date, note)
SELECT st.id, s.id, t.id, 5, 2, CURRENT_DATE - 7, 'Sprawdzian 1'
FROM students st
JOIN classes c ON c.id = st.class_id AND c.name='1A'
JOIN subjects s ON s.name='Matematyka'
JOIN teachers t ON t.last_name='Nowak'
LIMIT 1;

INSERT INTO grades (student_id, subject_id, teacher_id, value, weight, grade_date, note)
SELECT st.id, s.id, t.id, 4, 1, CURRENT_DATE - 3, 'Kartkówka'
FROM students st
JOIN classes c ON c.id = st.class_id AND c.name='1A'
JOIN subjects s ON s.name='Język polski'
JOIN teachers t ON t.last_name='Kowalski'
LIMIT 1;


-- Obecności
INSERT INTO attendance (student_id, subject_id, teacher_id, lesson_date, status, note)
SELECT st.id, s.id, t.id, CURRENT_DATE - 1, 'P', 'Obecny'
FROM students st
JOIN classes c ON c.id = st.class_id AND c.name='1A'
JOIN subjects s ON s.name='Matematyka'
JOIN teachers t ON t.last_name='Nowak'
LIMIT 2;


INSERT INTO attendance (student_id, subject_id, teacher_id, lesson_date, status, note)
SELECT st.id, s.id, t.id, CURRENT_DATE - 1, 'A', 'Nieobecny'
FROM students st
JOIN classes c ON c.id = st.class_id AND c.name='1B'
JOIN subjects s ON s.name='Matematyka'
JOIN teachers t ON t.last_name='Kowalski'
LIMIT 1;