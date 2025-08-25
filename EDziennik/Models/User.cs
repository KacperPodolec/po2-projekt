using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username nie może być pusty");
                _username = value;
            }
        }
        private string _username;

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PasswordHash nie może być pusty");
                _password = value;
            }
        }
        private string _password;

        public string Role
        {
            get => _role;
            set
            {
                if (value != "admin" && value != "teacher")
                    throw new ArgumentException("Rola musi być 'admin' lub 'teacher'");
                _role = value;
            }
        }
        private string _role;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
