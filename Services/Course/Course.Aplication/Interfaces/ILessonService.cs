using Course.Aplication.DTO;
using Course.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Aplication.Interfaces
{
    public interface ILessonService
    {
        Task<LessonReadDTO> CreateLessonAsync(int courseId, LessonDTO dto);
        Task<IEnumerable<LessonReadDTO>> GetLessonsByCourseAsync(int courseId);
        Task<LessonReadDTO?> GetLessonAsync(int id);
    }
}
