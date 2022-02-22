using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmsProcessingService.Domain.Entities;
using SmsProcessingService.Domain.Enums;
using SmsProcessingService.Domain.Settings;
using SmsProcessingService.Persistence;
using SmsProcessingService.Service.Contract;

namespace SmsProcessingService.Service.Implementation
{
    public class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly RabbitMqConfiguration _configuration;
        private readonly ILogger _logger;


        public ConsumerService(IRabbitMqService rabbitMqService, IApplicationDbContext applicationDbContext,
            IOptions<RabbitMqConfiguration> options, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<ConsumerService>();
            _configuration = options.Value;
            _applicationDbContext = applicationDbContext;
            Thread.Sleep(15000);
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_configuration.QueueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind(_configuration.QueueName, _configuration.Exchange, _configuration.RoutingKey);
            _logger.LogInformation($"Queue \"{_configuration.QueueName}\" is waiting for messages.");
        }

        public Task ReadMessages()
        {
            var messageCount = _model.MessageCount(_configuration.QueueName);
            if (messageCount > 0)
            {
                _logger.LogInformation($"\tDetected {messageCount} message(s).");
            }

            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);

                var sms = await SaveToDatabase(message);

                _logger.LogInformation(
                    $"[{DateTime.Now}] [From {sms.From}] Processing status is {sms.Status.ToString().ToLower()}" +
                    $"\r\nList of recipient numbers: {string.Join(", ", sms.To)}");

                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_configuration.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }

        private async Task<SmsEntity> SaveToDatabase(string message)
        {
            SmsEntity sms = null;

            if (message == null)
                return sms;

            try
            {
                sms = JsonSerializer.Deserialize<SmsEntity>(message);

                if (sms != null)
                {
                    sms.Id = Guid.NewGuid();
                    sms.Status = sms.To.All(c => Regex.Match(c, @"^((\+7|7|8|\+61)+([0-9]){10})$").Success)
                        ? SmsStatus.Delivered
                        : SmsStatus.Failed;

                    _applicationDbContext.SmsEntities.Add(sms);
                    await _applicationDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(default, ex, ex.Message);
            }

            return sms;
        }
    }
}