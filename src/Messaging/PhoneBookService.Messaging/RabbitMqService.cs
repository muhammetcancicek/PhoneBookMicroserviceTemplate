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


    public class RabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IModel _channel;

        public RabbitMqService(string connectionString = "amqp://guest:guest@s_rabbitmq:5672/")
        {
            var connectionStr = "amqp://guest:guest@s_rabbitmq:5672/";
            _connectionFactory = new ConnectionFactory() { Uri = new Uri(connectionStr) };
            InitializeRabbitMq();
            //asdas
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

        public void SendRequest<T>(string queueName, T message, string replyQueueName, string correlationId)
        {
            DeclareQueue(queueName);
            DeclareQueue(replyQueueName);
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: props,
                body: body);
        }

        public void StartReplyConsumer<T>(string queueName, Action<T, BasicDeliverEventArgs> onReplyReceived)
        {
            DeclareQueue(queueName);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                onReplyReceived?.Invoke(message, ea);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void SendResponse(string replyQueue, byte[] responseBytes, string correlationId)
        {
            DeclareQueue(replyQueue);
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;

            _channel.BasicPublish(
                exchange: "",
                routingKey: replyQueue,
                basicProperties: props,
                body: responseBytes);
        }
    }
}
