using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Domain.Entities
{
    public class Courses
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public bool IsPremium { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public decimal Price { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
