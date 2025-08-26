using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    // Reprezentuje przedmiot szkolny
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
}
