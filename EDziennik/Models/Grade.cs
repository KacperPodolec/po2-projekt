using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    // Model reprezentujący ocenę ucznia
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
}
