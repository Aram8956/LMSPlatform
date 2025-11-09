using Course.Aplication.DTO;
using Course.Aplication.Interfaces;
using Course.Domain.Entities;
using Course.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Aplication.Services
{
    public class CourseServ : ICourseService
    {
        private readonly CourseDbContext _context;
        public CourseServ(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<Courses> GetCourses(int id)
        {
            var course = await _context.Courses
               .Include(c => c.Lessons)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new Exception($"Course with id {id} not found");

            return course;
        }

        public async Task<IEnumerable<Courses>> GetAll()
        {
            return await _context.Courses
               .Include(c => c.Lessons)
               .ToListAsync();
        }

        public async Task<Courses> CreateAsync(CourseDTO dto)
        {
            var course = new Courses
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Level = dto.Level,
                Price = dto.Price,
                InstructorId = dto.InstructorId,
                IsPremium = dto.Price > 0,
                Lessons = new List<Lesson>()
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Courses> UpdateAsync(int id, CourseDTO dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                throw new Exception($"Course with id {id} not found");

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Category = dto.Category;
            course.Level = dto.Level;
            course.Price = dto.Price;
            course.IsPremium = dto.Price > 0;

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Lesson> AddLesson(int courseId, LessonDTO dto)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
                throw new Exception($"Course with id {courseId} not found");

            var lesson = new Lesson
            {
                Title = dto.Title,
                Content = dto.Content,
                Order = dto.Order,
                CourseId = courseId
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }
    }
}
