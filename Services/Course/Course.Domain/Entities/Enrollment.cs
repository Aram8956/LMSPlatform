using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Domain.Entities
{
    public class Enrollment
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
        public bool IsPaid { get; set; }
        public User User { get; set; }
        public Courses Course { get; set; }
    }
}
