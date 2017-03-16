

namespace Dsa.RapidResponse.Services
{
    public interface IEmailService
    {
        void Send(string to, string message);
    }
}