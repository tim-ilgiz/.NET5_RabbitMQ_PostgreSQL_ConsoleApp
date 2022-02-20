using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmsProcessingService.Domain.Enums;

namespace SmsProcessingService.Domain.Entities
{
    [Table("sms")]
    public class SmsEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("from")]
        public string From { get; set; }
        
        [Column("to")]
        public string[] To { get; set; }
        
        [Column("content")]
        public string Content { get; set; }
        
        [Column("status")]
        public SmsStatus Status { get; set; }
    }
}
