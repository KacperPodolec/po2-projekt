using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public DateTime LessonDate { get; set; }

        private char _status;
        public char Status
        {
            get => _status;
            set
            {
                if (value != 'P' && value != 'A' && value != 'E' && value != 'L')
                    throw new ArgumentException("Status musi być 'P','A','E' lub 'L'");
                _status = value;
            }
        }

        public string Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
