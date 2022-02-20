using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmsProcessingService.Service.Contract;
using System.Threading.Tasks;
using SmsProcessingService.Service.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/sms")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;
        
        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }
        
        [HttpGet]
        [Route("list")]
        public Task<IReadOnlyCollection<SmsDto>> GetAll()
        {
            return _smsService.GetAll();
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<SmsDto> GetById(Guid id)
        {
            return _smsService.GetById(id);
        }
        
        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest request)
        {
            await _smsService.SendSmsAsync(request);
            return Ok();
        }
    }
}