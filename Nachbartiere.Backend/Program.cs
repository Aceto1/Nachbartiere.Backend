using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Nachbartiere.Backend.Database;

namespace Nachbartiere.Backend
{
    public class Program
    {
        public static ConfigurationManager Configuration;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            Configuration = builder.Configuration;

            var conStr = builder.Configuration.GetConnectionString("Default");
            var path = builder.Configuration.GetValue<string>("ImageSavePath");

            if (conStr == null)
            {
                Console.WriteLine("ConnectionString not configured.");
                return;
            }

            DatabaseContext.ConnectionString = conStr;

            if (path == null)
            {
                Console.WriteLine("ImageSavePath not configured.");
                return;
            }

            builder.WebHost.UseSentry(o =>
            {
                o.Dsn = builder.Configuration.GetSection("Sentry").GetValue<string>("Dsn");
                // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://nachbartiere.eu.auth0.com/";
                options.Audience = "https://nachbartiere.de/api";
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseSentryTracing();
            app.UseAuthentication();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.MapControllers();
            
            app.UseRouting();
            app.UseAuthorization();

            app.Run();
        }
    }
}