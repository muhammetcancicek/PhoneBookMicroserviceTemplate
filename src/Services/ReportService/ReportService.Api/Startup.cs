using ReportService.Infrastructure;
using MongoDB.Driver;
using ReportService.Application.Services;
using ReportService.Application.Services.Interfaces;
using ReportService.Domain.Interfaces;
using ReportService.Infrastructure.Repositories;
using PhoneBookService.Infrastructure.Repositoryes;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using PhoneBookService.Messaging;

namespace ReportService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IMongoClient>(provider =>
            new MongoClient(Configuration.GetConnectionString("MongoDb")));

            services.AddSingleton<IMongoDatabase>(provider =>
                provider.GetRequiredService<IMongoClient>().GetDatabase("PhoneBookDb"));
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IReportService, ReportService.Application.Services.ReportService>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
