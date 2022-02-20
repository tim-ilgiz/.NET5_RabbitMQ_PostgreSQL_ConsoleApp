using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmsProcessingService.Service.Common.Exceptions;

namespace SmsProcessingService.Infrastructure.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex,
            ILogger<CustomExceptionMiddleware> logger)
        {
            var code = (HttpStatusCode)StatusCodes.Status400BadRequest; ; // 400 if unexpected

            switch (ex)
            {
                case BadRequestException:
                    code = (HttpStatusCode)StatusCodes.Status400BadRequest;
                    break;
            }

            logger.LogError(ex.Message);

            var result = JsonConvert.SerializeObject(new { StatusCode = (int)code, ErrorMessage = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            
            return context.Response.WriteAsync(result);
        }
    }
}