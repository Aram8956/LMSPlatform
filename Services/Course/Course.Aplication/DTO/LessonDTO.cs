using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Aplication.DTO
{
    public class LessonDTO
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Order { get; set; }
    }
}
