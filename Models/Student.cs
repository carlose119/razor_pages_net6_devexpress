using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace razor_pages_net6.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}