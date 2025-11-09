using Course.Aplication.Interfaces;
using Course.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Course.Aplication.Interfaces;
using Course.Infrastructure;
using Shared.Events;

namespace Course.Aplication.Services
{
    public class EnrollmentServ: IEnrollment
    {
        private readonly CourseDbContext db;
        private readonly IBilling billing;
        private readonly IPublishEndpoint bus;
        public EnrollmentServ(CourseDbContext db, IBilling billing, IPublishEndpoint bus)
        {
            this.db = db;
            this.billing = billing;
            this.bus = bus;
        }

        public async Task<Enrollment> EnrollAsync(int userId, int courseId, CancellationToken ct = default)
        {
            using var tx = await db.Database.BeginTransactionAsync(ct);

            var course = await db.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Id == courseId, ct)
                         ?? throw new InvalidOperationException("Course not found");

            if (course.IsPremium)
            {
                var paid = await billing.HasPaidAsync(userId, courseId, ct);
                if (!paid) throw new InvalidOperationException("Payment required");
            }

            var exists = await db.Enrollments.FindAsync(new object[] { userId, courseId }, ct) != null;
            if (exists) throw new InvalidOperationException("Already enrolled");

            var enrollment = new Enrollment { UserId = userId, CourseId = courseId, IsPaid = course.IsPremium };
            db.Enrollments.Add(enrollment);

            var firstLessonId = course.Lessons.OrderBy(l => l.Order).Select(l => l.Id).FirstOrDefault();
            if (firstLessonId != 0)
            {
                db.LessonProgress.Add(new LessonProgress { UserId = userId, LessonId = firstLessonId });
            }

            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            await bus.Publish(new EnrollmentCreated(userId, courseId, DateTime.UtcNow), ct);
            return enrollment;
        }
    }
}
