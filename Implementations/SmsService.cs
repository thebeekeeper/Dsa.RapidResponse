using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using Dsa.RapidResponse.Services;

namespace Dsa.RapidResponse.Implementations
{
    public class SmsService : IMessagingService
    {
        async void IMessagingService.SendMessage(string message)
        {
#if RELEASE
            var number = "";
            var values = new Dictionary<string, string>
            {
                { "number", number },
                { "message", message },
                { "key", "key goes here" },
            };

            var content = new FormUrlEncodedContent(values);
            var c = new HttpClient();
            var resp = await c.PostAsync("https://textbelt.com/text", new FormUrlEncodedContent(values));
            Console.WriteLine(resp.StatusCode);
#endif
        }
    }
}