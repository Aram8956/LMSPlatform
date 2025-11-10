using Billing.API.Messeges;
using Billing.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Polly;

namespace Billing.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var b = WebApplication.CreateBuilder(args);

            b.Services.AddDbContext<BillingContext>(o =>
                o.UseSqlServer(b.Configuration.GetConnectionString("BillingDb")));

            b.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CourseCompletedConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(b.Configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(b.Configuration["RabbitMQ:User"]);
                        h.Password(b.Configuration["RabbitMQ:Pass"]);
                    });
                    cfg.ReceiveEndpoint("billing.course-completed", e =>
                    {
                        e.ConfigureConsumer<CourseCompletedConsumer>(ctx);
                    });
                });
            });

            b.Services.AddControllers();
            b.Services.AddEndpointsApiExplorer();
            b.Services.AddSwaggerGen();

            var app = b.Build();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<BillingContext>();

            //    var retry = Policy
            //        .Handle<Exception>()
            //        .WaitAndRetry(10, _ => TimeSpan.FromSeconds(5)); // повторы каждые 5 сек

            //    retry.Execute(() =>
            //    {
            //        Console.WriteLine("⏳ Trying to migrate BillingDB...");
            //        db.Database.Migrate();
            //        Console.WriteLine("✅ BillingDB migrated!");
            //    });
            //}

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();

            app.Run();
        }
    }
}
