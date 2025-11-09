using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Aplication.DTO
{
    public class LessonReadDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Order { get; set; }
    }
}
