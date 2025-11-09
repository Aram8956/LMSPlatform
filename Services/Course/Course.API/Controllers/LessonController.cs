using Course.Aplication.DTO;
using Course.Aplication.Interfaces;
using Course.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> Create(int courseId, LessonDTO dto)
        {
            var result = await _lessonService.CreateLessonAsync(courseId, dto);
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var lessons = await _lessonService.GetLessonsByCourseAsync(courseId);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lesson = await _lessonService.GetLessonAsync(id);
            return lesson is null ? NotFound() : Ok(lesson);
        }
    }
}
