using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SmsProcessingService.Domain.Settings;
using SmsProcessingService.Service.Contract;
using System;

namespace SmsProcessingService.Service.Implementation
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMqConfiguration _configuration;

        public RabbitMqService(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }

        public IConnection CreateChannel()
        {
            var connection = new ConnectionFactory()
            {
                UserName = _configuration.Username,
                Password = _configuration.Password,
                HostName = _configuration.HostName,
                VirtualHost =_configuration.VirtualHost,
                Port = Convert.ToInt32(_configuration.Port)
            };
            connection.DispatchConsumersAsync = true;
            connection.RequestedHeartbeat = new System.TimeSpan(0, 0, 60);
            var channel = connection.CreateConnection();

            return channel;
        }
    }
}