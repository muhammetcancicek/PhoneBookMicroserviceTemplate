﻿using AutoMapper;
using MongoDB.Driver;
using PhoneBookService.Application;
using PhoneBookService.Application.AutoMapper;
using PhoneBookService.Application.Services;
using PhoneBookService.Application.Services.Interfaces;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using PhoneBookService.Infrastructure;
using PhoneBookService.Infrastructure.Repositoryes;

namespace PhoneBookService.Api
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
            services.AddScoped<MappingService>();

            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IContactInfoService, ContactInfoService>();


            services.AddSingleton<IMongoClient>(provider =>
            new MongoClient(Configuration.GetConnectionString("MongoDb")));

            services.AddSingleton<IMongoDatabase>(provider =>
                provider.GetRequiredService<IMongoClient>().GetDatabase("PhoneBookDb"));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IContactInfoService, ContactInfoService>();
            services.AddScoped<IPersonService, PersonService>();

            services.AddAutoMapper(typeof(MappingProfile));
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

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
