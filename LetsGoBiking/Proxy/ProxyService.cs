using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Proxy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class ProxyService : IProxyService

    {
        static readonly HttpClient client = new HttpClient();

        private Cache cache = new Cache();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public APIResponse Call(string url)
        {
            APIResponse apiResponse = cache.GetRouteCache(url);

            if (apiResponse == null)
            {
                apiResponse = new APIResponse();
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    apiResponse.Status = (int)response.StatusCode;
                    apiResponse.Reponse = responseBody;

                    cache.SetRouteCache(url, apiResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);

                    apiResponse.Status = 500;
                }
            }

            return apiResponse;
        }
    }
}
