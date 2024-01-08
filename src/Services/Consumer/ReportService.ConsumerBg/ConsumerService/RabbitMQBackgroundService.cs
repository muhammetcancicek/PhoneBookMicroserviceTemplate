using MongoDB.Bson.IO;
using PhoneBookService.Messaging;
using RabbitMQ.Client.Events;
using ReportService.Application.Services.Interfaces;
using System.Text;
using Microsoft.Extensions.Hosting; 
using Newtonsoft.Json; 
namespace ReportService.ConsumerBg.ConsumerService
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IReportService _reportService;
        private readonly HttpClient _httpClient; 

        public RabbitMQBackgroundService(IReportService reportService, HttpClient httpClient)
        {
            _reportService = reportService;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            _httpClient = new HttpClient(handler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = "report_requests";
            var rabbitMqService = new RabbitMqService();
            rabbitMqService.StartReplyConsumer<Guid>(queueName, (reportId, ea) =>
            {
                ProcessMessage(reportId, ea);
            });
            Console.WriteLine("Consumer başlatıldı. Mesajları dinliyor...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        private async Task ProcessMessage(Guid reportId, BasicDeliverEventArgs ea)
        {
            var rabbitMqService = new RabbitMqService();
            try
            {
                var reportCreateUrl = $"http://reportservice.api:5050/api/Report/CreateReport/{reportId}";
                var response = await _httpClient.PostAsync(reportCreateUrl, null);

                string responseMessage;
                if (response.IsSuccessStatusCode)
                {
                    responseMessage = $"Rapor API'ye başarıyla gönderildi : {reportId}";
                }
                else
                {
                    responseMessage = $"Rapor API'ye gönderilirken hata oluştu : {reportId}";
                }

                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                rabbitMqService.SendResponse(ea.BasicProperties.ReplyTo, responseBytes, ea.BasicProperties.CorrelationId);
                Console.WriteLine(responseMessage);
            }
            catch (Exception ex)
            {
                var errorResponseMessage = $"Rapor oluşturulma işleminde hata: {ex.Message}";
                var errorResponseBytes = Encoding.UTF8.GetBytes(errorResponseMessage);
                rabbitMqService.SendResponse(ea.BasicProperties.ReplyTo, errorResponseBytes, ea.BasicProperties.CorrelationId);
                Console.WriteLine(errorResponseMessage);
            }
        }
    }
}
