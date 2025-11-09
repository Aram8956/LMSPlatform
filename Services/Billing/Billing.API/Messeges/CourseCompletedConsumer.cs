using MassTransit;
using Shared.Events;

namespace Billing.API.Messeges
{
    public class CourseCompletedConsumer : IConsumer<CourseCompleted>
    {
        public Task Consume(ConsumeContext<CourseCompleted> context)
        {
            Console.WriteLine($"[Billing] CourseCompleted: user={context.Message.UserId}, course={context.Message.CourseId}");
            return Task.CompletedTask;
        }
    }
}