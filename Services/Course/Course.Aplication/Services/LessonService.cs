using Course.Aplication.Interfaces;
using Course.Domain.Entities;
using Course.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Course.Aplication.DTO;

namespace Course.Aplication.Services
{
    public class LessonService : ILessonService
    {
        private readonly CourseDbContext _db;

        public LessonService(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<LessonReadDTO> CreateLessonAsync(int courseId, LessonDTO dto)
        {
            var lesson = new Lesson
            {
                CourseId = courseId,
                Title = dto.Title,
                Content = dto.Content,
                Order = dto.Order
            };

            _db.Lessons.Add(lesson);
            await _db.SaveChangesAsync();

            return new LessonReadDTO
            {
                Id = lesson.Id,
                CourseId = courseId,
                Title = lesson.Title,
                Content = lesson.Content,
                Order = lesson.Order
            };
        }

        public async Task<IEnumerable<LessonReadDTO>> GetLessonsByCourseAsync(int courseId)
        {
            return await _db.Lessons
                .Where(x => x.CourseId == courseId)
                .Select(x => new LessonReadDTO
                {
                    Id = x.Id,
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Content = x.Content,
                    Order = x.Order
                })
                .ToListAsync();
        }

        public async Task<LessonReadDTO?> GetLessonAsync(int id)
        {
            var x = await _db.Lessons.FindAsync(id);
            if (x == null) return null;

            return new LessonReadDTO
            {
                Id = x.Id,
                CourseId = x.CourseId,
                Title = x.Title,
                Content = x.Content,
                Order = x.Order
            };
        }
    }
}
