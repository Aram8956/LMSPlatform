using Course.Aplication.DTO;
using Course.Aplication.Interfaces;
using Course.Domain.Entities;
using Course.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAll();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var course = await _service.GetCourses(id);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removed = await _service.DeleteAsync(id);
            if (!removed) return NotFound();

            return Ok("Deleted");
        }

        [HttpPost("{id}/lessons")]
        public async Task<IActionResult> AddLesson(int id, [FromBody] LessonDTO dto)
        {
            var lesson = await _service.AddLesson(id, dto);
            return Ok(lesson);
        }
    }
}
