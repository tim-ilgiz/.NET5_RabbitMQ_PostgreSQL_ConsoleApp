using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmsProcessingService.Service.Models
{
    public class SmsDto
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string[] To { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public string Status { get; set; }
    }
}
