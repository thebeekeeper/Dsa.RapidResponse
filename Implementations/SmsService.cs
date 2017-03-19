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
        async void IMessagingService.SendMessage(string destination, string message)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("TEXTBELT_APIKEY");

            if (string.IsNullOrEmpty(apiKey) == false)
            {

                var values = new Dictionary<string, string>
                {
                    { "number", destination },
                    { "message", message },
                    { "key", apiKey },
                };
                var content = new FormUrlEncodedContent(values);
                var c = new HttpClient();
                var resp = await c.PostAsync("https://textbelt.com/text", new FormUrlEncodedContent(values));
                Console.WriteLine(resp.Content.ReadAsStringAsync().Result);
            }
        }
    }
}