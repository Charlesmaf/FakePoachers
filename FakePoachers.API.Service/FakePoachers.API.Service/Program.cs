using FakePoachers.Infrastructure.Contracts;
using FakePoachers.Infrastructure.Models;
using FakePoachers.Library.Service;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FakePoachers.API.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var allowedOrigins = "AllowedOrigins";

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IAnimalService, AnimalService>();
            builder.Services.AddTransient<ILocationService, LocationService>();
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>(); // Brotli LocationService
                options.Providers.Add<GzipCompressionProvider>();  // Gzip
            });
            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });
            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });

            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()
                             .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                             .CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FakePoachers.API.Service")));

            builder.Services.AddMemoryCache();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowedOrigins, policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
            var app = builder.Build();
            app.UseSerilogRequestLogging();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(allowedOrigins);
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
