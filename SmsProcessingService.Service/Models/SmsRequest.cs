namespace SmsProcessingService.Service.Models
{
    public class SmsRequest
    {
        public string From { get; set; }
        public string[] To { get; set; }
        public string Content { get; set; }
    }
}