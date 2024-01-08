using ReportService.Infrastructure;
using MongoDB.Driver;
using ReportService.Application.Services;
using ReportService.Application.Services.Interfaces;
using ReportService.Domain.Interfaces;
using ReportService.Infrastructure.Repositories;
using PhoneBookService.Infrastructure.Repositoryes;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using ReportService.ConsumerBg.ConsumerService;

namespace ReportService.ConsumerBg
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
new MongoClient("mongodb+srv://devDbUser:Mcan1973!?@devdatabase.trskelc.mongodb.net/?retryWrites=true&w=majority"));

            services.AddSingleton<IMongoDatabase>(provider =>
                provider.GetRequiredService<IMongoClient>().GetDatabase("PhoneBookDb"));

            services.AddSingleton<IReportRepository, ReportRepository>();
            services.AddSingleton<IContactInfoRepository, ContactInfoRepository>();
            services.AddSingleton<IReportService, ReportService.Application.Services.ReportService>();
            services.AddHostedService<RabbitMQBackgroundService>();
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
