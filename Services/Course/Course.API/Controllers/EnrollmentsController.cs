using Course.Aplication.Interfaces;
using Course.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class EnrollmentsController(IEnrollment svc) : ControllerBase
{
    [Authorize]
    [HttpPost("enroll/{courseId:int}")]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);
        var result = await svc.EnrollAsync(userId, courseId);
        return Ok(new { result.UserId, result.CourseId, result.EnrolledAt, result.IsPaid });
    }

    [Authorize]
    [HttpGet("my-courses")]
    public IActionResult MyCourses([FromServices] CourseDbContext db)
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);
        var courses = db.Enrollments.Where(e => e.UserId == userId)
            .Select(e => new { e.CourseId, e.Course.Title, e.Course.Category, e.Course.Level })
            .ToList();
        return Ok(courses);
    }
}
