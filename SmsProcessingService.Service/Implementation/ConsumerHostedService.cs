using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SmsProcessingService.Service.Contract;

namespace SmsProcessingService.Service.Implementation
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public ConsumerHostedService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _consumerService.ReadMessages();
        }
    }
}