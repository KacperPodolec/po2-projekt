using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDziennik.Models
{
    public class StudentListItem
    {
        public int Id { get; set; }          
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public DateTime BirthDate { get; set; }
        public int ClassId { get; set; }   
        public string ClassName { get; set; }
    }
}
