using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    public class Teacher
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

        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
