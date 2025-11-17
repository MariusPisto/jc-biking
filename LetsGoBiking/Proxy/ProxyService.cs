using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Proxy
{
    public class ProxyService : IProxyService

    {
        static readonly HttpClient client = new HttpClient();

        private Cache cache = new Cache();

        public APIResponse CallGet(string url)
        {
            Console.WriteLine(url + ": requested");
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
                    apiResponse.Response = responseBody;

                    cache.SetRouteCache(url, apiResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);

                    apiResponse.Response = e.Message;
                    apiResponse.Status = 500;
                }
            }

            Console.WriteLine(url + ": sent");
            return apiResponse;
        }

        public APIResponse CallPost(string url, string jsonBody)
        {
            Console.WriteLine("POST " + url + ": requested");

            string cacheKey = url + jsonBody;

            APIResponse apiResponse = cache.GetRouteCache(cacheKey);

            if (apiResponse == null)
            {
                apiResponse = new APIResponse();
                try
                {
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    apiResponse.Status = (int)response.StatusCode;
                    apiResponse.Response = responseBody;

                    cache.SetRouteCache(cacheKey, apiResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    apiResponse.Response = e.Message;
                    apiResponse.Status = 500;
                }
            }
            else
            {
                Console.WriteLine("POST " + url + ": retrieved from cache");
            }

            Console.WriteLine("POST " + url + ": sent");
            return apiResponse;
        }
    }
}
