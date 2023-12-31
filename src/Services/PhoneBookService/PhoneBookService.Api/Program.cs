using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using PhoneBookService.Api;
using PhoneBookService.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);




var mongoClient = app.Services.GetRequiredService<IMongoClient>();
var database = mongoClient.GetDatabase("PhoneBookDb");

DataSeeder.SeedData(database);

app.Run();
