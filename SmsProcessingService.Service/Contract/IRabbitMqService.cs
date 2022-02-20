using RabbitMQ.Client;

namespace SmsProcessingService.Service.Contract
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}