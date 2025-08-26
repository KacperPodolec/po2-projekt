using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    // Model reprezentujący klasę szkolną
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
}
