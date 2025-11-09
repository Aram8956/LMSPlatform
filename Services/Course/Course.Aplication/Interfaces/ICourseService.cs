using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Domain.Entities;
using Course.Aplication.DTO;


namespace Course.Aplication.Interfaces
{
    public interface ICourseService
    {
        Task<Courses> GetCourses(int id);
        Task<IEnumerable<Courses>> GetAll();
        Task<Courses> CreateAsync(CourseDTO course);
        Task<Courses> UpdateAsync(int id, CourseDTO course);
        Task<bool> DeleteAsync(int id);
        Task<Lesson> AddLesson(int courseId, LessonDTO dto);
    }
}
