using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Domain.Entities
{
    public class LessonProgress
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime CompletedAt { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
