using System;
using System.Collections.Generic;
using SmsProcessingService.Domain.Entities;
using SmsProcessingService.Service.Models;
using System.Threading.Tasks;

namespace SmsProcessingService.Service.Contract
{
    public interface ISmsService
    {
        Task SendSmsAsync(SmsRequest incomingSms);
        Task<Guid> SaveSmsAsync(SmsEntity processedSms);
        Task<IReadOnlyCollection<SmsDto>> GetAll();
        Task<SmsDto> GetById(Guid id);
    }
}
