using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }

        private short _value;
        public short Value
        {
            get => _value;
            set
            {
                if (value < 1 || value > 6)
                    throw new ArgumentException("Value musi być 1–6");
                _value = value;
            }
        }

        private short _weight;
        public short Weight
        {
            get => _weight;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Weight musi być 1–5");
                _weight = value;
            }
        }

        public DateTime GradeDate { get; set; } = DateTime.Today;
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
