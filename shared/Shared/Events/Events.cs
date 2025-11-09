using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public record CourseCompleted(int UserId, int CourseId, DateTime CompletedAt);
    public record EnrollmentCreated(int UserId, int CourseId, DateTime EnrolledAt);
}
