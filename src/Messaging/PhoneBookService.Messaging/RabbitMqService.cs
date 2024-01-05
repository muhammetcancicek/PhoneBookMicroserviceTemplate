using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhoneBookService.Messaging
{
    public interface IRabbitMqService
    {
        void PublishMessage<T>(string queueName, T message);
        void StartConsumer<T>(string queueName, Action<T> onMessageReceived);

    }

    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IModel _channel;

        public RabbitMqService(string connectionString)
        {
            _connectionFactory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
            var connection = _connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void PublishMessage<T>(string queueName, T message)
        {
            DeclareQueue(queueName);

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void StartConsumer<T>(string queueName, Action<T> onMessageReceived)
        {
            DeclareQueue(queueName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                onMessageReceived?.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private void DeclareQueue(string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }
    }
}
