using Microsoft.Extensions.DependencyInjection;
using PhoneBookService.Messaging;
using ReportService.Application;
using ReportService.Application.Services;
using ReportService.Application.Services.Interfaces;
using System;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var rabbitMqService = serviceProvider.GetService<IRabbitMqService>();
        var reportService = serviceProvider.GetService<IReportService>();


        StartConsumer(rabbitMqService, reportService);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var connectionString = "amqp://guest:guest@localhost:5672/";
        services.AddSingleton<IRabbitMqService, RabbitMqService>(_ => new RabbitMqService(connectionString));
        services.AddSingleton<IReportService, ReportService.Application.Services.ReportService>();
    }

    private static void StartConsumer(IRabbitMqService rabbitMqService, IReportService reportService)
    {
        var queueName = "report_requests";
        rabbitMqService.StartConsumer<Guid>(queueName, reportId =>
        {
            ProcessMessage(reportId, rabbitMqService, reportService);
        });
        Console.WriteLine("Consumer başlatıldı. Mesajları dinliyor...");
        Console.ReadLine(); 
    }

    private async static void ProcessMessage(Guid reportId, IRabbitMqService rabbitMqService, IReportService reportService)
    {
        try
        { 
            await reportService.CreateReport(reportId);

            // Rapor güncellendikten sonra RabbitMQ'ya bir cevap mesajı gönder
            rabbitMqService.PublishMessage("report_responses", $"İşlem başarılı. Rapor ID = {reportId}");

            Console.WriteLine($"Rapor başarıyla oluşturuldu : {reportId}");
        }
        catch (Exception ex)
        {
            rabbitMqService.PublishMessage("report_responses", $"İşlem başarısız!");
            Console.WriteLine($"Rapor oluşturulma işleminde hata: {ex.Message}");
        }
    }
}
