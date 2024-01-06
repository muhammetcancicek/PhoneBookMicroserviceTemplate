using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using PhoneBookService.Infrastructure.Repositoryes;
using PhoneBookService.Messaging;
using RabbitMQ.Client.Events;
using ReportService.Application;
using ReportService.Application.Services;
using ReportService.Application.Services.Interfaces;
using ReportService.Domain.Interfaces;
using ReportService.Infrastructure.Repositories;
using System;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var reportService = serviceProvider.GetService<IReportService>();

        StartConsumer(reportService);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var connectionString = "amqp://guest:guest@s_rabbitmq:5672/";

        services.AddSingleton<IReportService, ReportService.Application.Services.ReportService>();
        services.AddSingleton<IReportRepository, ReportRepository>();

        services.AddSingleton<IMongoClient>(provider =>
new MongoClient("mongodb+srv://devDbUser:Mcan1973!?@devdatabase.trskelc.mongodb.net/?retryWrites=true&w=majority"));

        services.AddSingleton<IMongoDatabase>(provider =>
            provider.GetRequiredService<IMongoClient>().GetDatabase("PhoneBookDb"));
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
        services.AddScoped<IReportService, ReportService.Application.Services.ReportService>();
    }

    private static void StartConsumer(IReportService reportService)
    {
        var rabbitMqService = new RabbitMqService();
        var queueName = "report_requests";
        rabbitMqService.StartReplyConsumer<Guid>(queueName, (reportId, ea) =>
        {
            ProcessMessage(reportId, reportService, ea);
        });
        Console.WriteLine("Consumer başlatıldı. Mesajları dinliyor...");
        Console.ReadLine();
    }

    private async static void ProcessMessage(Guid reportId, IReportService reportService, BasicDeliverEventArgs ea)
    {
            var rabbitMqService = new RabbitMqService();
        try
        {
            await reportService.CreateReport(reportId);

            // Rapor güncellendikten sonra RabbitMQ'ya bir cevap mesajı gönder
            var responseMessage = $"İşlem başarılı. Rapor ID = {reportId}";
            var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            rabbitMqService.SendResponse(ea.BasicProperties.ReplyTo, responseBytes, ea.BasicProperties.CorrelationId);

            Console.WriteLine($"Rapor başarıyla oluşturuldu : {reportId}");
        }
        catch (Exception ex)
        {
            var errorResponseMessage = $"İşlem başarısız!";
            var errorResponseBytes = Encoding.UTF8.GetBytes(errorResponseMessage);
            rabbitMqService.SendResponse(ea.BasicProperties.ReplyTo, errorResponseBytes, ea.BasicProperties.CorrelationId);

            Console.WriteLine($"Rapor oluşturulma işleminde hata: {ex.Message}");
        }
    }
}
