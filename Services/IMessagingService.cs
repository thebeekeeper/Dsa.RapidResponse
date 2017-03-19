
namespace Dsa.RapidResponse.Services
{
    public interface IMessagingService
    {
        void SendMessage(string destination, string message);
    }
}