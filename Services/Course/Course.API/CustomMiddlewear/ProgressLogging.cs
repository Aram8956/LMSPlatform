using Course.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Course.API.CustomMiddlewear
{
    public class ProgressLogging
    {
        private readonly RequestDelegate _next;
        public ProgressLogging(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext ctx, CourseDbContext db)
        {
            if (ctx.Request.Path.StartsWithSegments("/lessons") &&
                ctx.Request.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase) &&
                ctx.User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(ctx.User.FindFirst("sub")!.Value);
                if (int.TryParse(ctx.Request.RouteValues["id"]?.ToString(), out var lessonId))
                {
                    var progress = await db.LessonProgress.FindAsync(userId, lessonId);
                    if (progress is null)
                    {
                        progress = new() { UserId = userId, LessonId = lessonId };
                        db.LessonProgress.Add(progress);
                    }
                    if (progress.CompletedAt == default)
                    {
                        progress.CompletedAt = DateTime.UtcNow;
                    }
                    await db.SaveChangesAsync();

                    var courseId = await db.Lessons.Where(l => l.Id == lessonId).Select(l => l.CourseId).SingleAsync();
                    var lessonIds = await db.Lessons.Where(l => l.CourseId == courseId).Select(l => l.Id).ToListAsync();
                    var completed = await db.LessonProgress.Where(p => p.UserId == userId && p.CompletedAt != null && lessonIds.Contains(p.LessonId))
                        .Select(p => p.LessonId).Distinct().CountAsync();
                    if (completed == lessonIds.Count && lessonIds.Count > 0)
                    {
                        var bus = ctx.RequestServices.GetRequiredService<IPublishEndpoint>();
                        await bus.Publish(new CourseCompleted(userId, courseId, DateTime.UtcNow));
                    }
                }
            }
            await _next(ctx);
        }
    }
}
