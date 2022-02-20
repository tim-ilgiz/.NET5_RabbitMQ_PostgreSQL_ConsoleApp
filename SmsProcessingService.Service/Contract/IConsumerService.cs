using System.Threading.Tasks;

namespace SmsProcessingService.Service.Contract
{
    public interface IConsumerService
    {
        Task ReadMessages();
    }
}