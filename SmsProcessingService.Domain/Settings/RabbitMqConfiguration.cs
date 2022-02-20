namespace SmsProcessingService.Domain.Settings
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
        public string Port { get; set; }
    }
}