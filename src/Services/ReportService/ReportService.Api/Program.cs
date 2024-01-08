using MongoDB.Driver;
using ReportService.Api;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);




var mongoClient = app.Services.GetRequiredService<IMongoClient>();
var database = mongoClient.GetDatabase("PhoneBookDb");

app.Run();
