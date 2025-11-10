using Course.API.CustomMiddlewear;
using Course.API.HTTPClient;
using Course.Aplication.Interfaces;
using Course.Aplication.Services;
using Course.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Course.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CourseDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("CourseDb")));

            builder.Services.AddScoped<IEnrollment, EnrollmentServ>();
            builder.Services.AddScoped<ICourseService, CourseServ>(); 
            builder.Services.AddScoped<ILessonService, LessonService>();  
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddHttpClient<IBilling, BillingClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Billing:BaseUrl"]!);
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(Program).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        builder.Configuration["RabbitMQ:Host"]!,
                        h =>
                        {
                            h.Username(builder.Configuration["RabbitMQ:User"]!);
                            h.Password(builder.Configuration["RabbitMQ:Pass"]!);
                        });

                    cfg.ConfigureEndpoints(context);
                });
            });


            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization(o =>
            {
                o.AddPolicy("CanManageCourses", p => p.RequireRole("Instructor", "Admin"));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ProgressLogging>();

            app.MapControllers();

            app.Run();
        }
    }
}
