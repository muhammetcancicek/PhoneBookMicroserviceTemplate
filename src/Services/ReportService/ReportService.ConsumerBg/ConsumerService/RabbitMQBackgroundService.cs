using PhoneBookService.Messaging;
using RabbitMQ.Client.Events;
using ReportService.Application.Services.Interfaces;
using System.Text;

namespace ReportService.ConsumerBg.ConsumerService
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IReportService _reportService;

        public RabbitMQBackgroundService(IReportService reportService)
        {
            _reportService = reportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = "report_requests";
            var rabbitMqService = new RabbitMqService();
            rabbitMqService.StartReplyConsumer<Guid>(queueName, (reportId, ea) =>
            {
                ProcessMessage(reportId, _reportService, ea);
            });
            Console.WriteLine("Consumer başlatıldı. Mesajları dinliyor...");

            // Arka plan servisini sonsuz bir döngüde tutun
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        private async void ProcessMessage(Guid reportId, IReportService reportService, BasicDeliverEventArgs ea)
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
}
