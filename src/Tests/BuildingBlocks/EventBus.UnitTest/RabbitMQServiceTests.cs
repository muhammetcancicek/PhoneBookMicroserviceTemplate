using Moq;
using PhoneBookService.Messaging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    public class RabbitMqServiceTests
    {
        private readonly Mock<IModel> _mockChannel;
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IConnectionFactory> _mockConnectionFactory;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly string _mockConnectionString = "amqp://guest:guest@localhost:5672/";
        public RabbitMqServiceTests()
        {
            _mockChannel = new Mock<IModel>();
            _mockConnection = new Mock<IConnection>();
            _mockConnectionFactory = new Mock<IConnectionFactory>();

            _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);

            _rabbitMqService = new RabbitMqService(_mockConnectionString);
        }
        [Fact]
        public void PublishMessage_ShouldPublishToCorrectQueue()
        {
            // Arrange
            var queueName = "test_queue";
            var testMessage = "Test Message";
            var expectedBody = Encoding.UTF8.GetBytes(testMessage);

            // Act
            _rabbitMqService.PublishMessage(queueName, testMessage);

            // Assert
            _mockChannel.Verify(
                channel => channel.BasicPublish(
                    It.IsAny<string>(),
                    queueName,
                    It.IsAny<bool>(),
                    It.IsAny<IBasicProperties>(),
                    It.Is<ReadOnlyMemory<byte>>(bytes => bytes.ToArray().SequenceEqual(expectedBody))
                ),
                Times.Once
            );
        }
        [Fact]
        public void StartConsumer_ShouldConsumeFromCorrectQueue()
        {
            // Arrange
            var queueName = "test_queue";
            Action<string> onMessageReceived = (message) => { };

            // Act
            _rabbitMqService.StartConsumer(queueName, onMessageReceived);

            // Assert
            _mockChannel.Verify(
                channel => channel.BasicConsume(
                    queueName,
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<IDictionary<string, object>>(),
                    It.IsAny<IBasicConsumer>()),
                Times.Once
            );
        }
    }
}
