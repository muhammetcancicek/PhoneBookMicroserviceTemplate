

namespace ReportService.ConsumerBg
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseUrls("http://*:5100"); ;
                });
    }
}


//using MongoDB.Driver;
//using ReportService.ConsumerBg;

//var builder = WebApplication.CreateBuilder(args);

//var startup = new Startup(builder.Configuration);
//startup.ConfigureServices(builder.Services);

//var app = builder.Build();
//startup.Configure(app, app.Environment);

//var mongoClient = app.Services.GetRequiredService<IMongoClient>();
//var database = mongoClient.GetDatabase("PhoneBookDb");

//app.Run();