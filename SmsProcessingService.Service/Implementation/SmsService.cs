using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsProcessingService.Domain.Entities;
using SmsProcessingService.Domain.Settings;
using SmsProcessingService.Persistence;
using SmsProcessingService.Service.Contract;
using SmsProcessingService.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmsProcessingService.Service.Common.Exceptions;

namespace SmsProcessingService.Service.Implementation
{
    public class SmsService : ISmsService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly RabbitMqConfiguration _configuration;
        
        public ILogger<SmsService> Logger { get; }

        public SmsService(ILogger<SmsService> logger,
            IApplicationDbContext dbContext, IRabbitMqService rabbitMqService,
            IOptions<RabbitMqConfiguration> options)
        {
            Logger = logger;
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
            _configuration = options.Value;
        }
        public Task SendSmsAsync(SmsRequest incomingSms)
        {
            using var connection = _rabbitMqService.CreateChannel();
            using var model = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(incomingSms));
            model.BasicPublish(_configuration.Exchange,
                _configuration.RoutingKey, true, null,body);
            
            return Task.CompletedTask;
        }

        public Task<Guid> SaveSmsAsync(SmsEntity incomingSms)
        {
            _dbContext.SmsEntities.Add(incomingSms);
            var savedEntityCount = _dbContext.SaveChangesAsync();

            if (savedEntityCount.Result == 0)
                throw new BadRequestException("Failed to save sms");

            return Task.FromResult(incomingSms.Id);
        }
        
        public Task<IReadOnlyCollection<SmsDto>> GetAll()
        {
            var smsEntities = _dbContext.SmsEntities.Select(c => new SmsDto
            {
                Id = c.Id,
                From = c.From,
                To = c.To,
                Status = c.Status.ToString()
            }).ToArray();

            return Task.FromResult<IReadOnlyCollection<SmsDto>>(smsEntities);
        }

        public Task<SmsDto> GetById(Guid id)
        {
            var smsEntity = _dbContext.SmsEntities.Find(id);
            
            if (smsEntity == null)
                throw new BadRequestException($"Sms with id = {id} not found");
            
            var sms = new SmsDto
            {
                Id = smsEntity.Id,
                From = smsEntity.From,
                To = smsEntity.To,
                Status = smsEntity.Status.ToString()
            };

            return Task.FromResult(sms);
        }
    }
}