using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy
{
    public class ProxyService : IProxyService

    {
        static readonly HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

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
                    Console.WriteLine($"{url}: Making HTTP request...");
                    
                    // Use ConfigureAwait(false) to avoid deadlocks and GetResult() to block synchronously
                    HttpResponseMessage response = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    apiResponse.Status = (int)response.StatusCode;
                    apiResponse.Response = responseBody;

                    cache.SetRouteCache(url, apiResponse);
                    Console.WriteLine($"{url}: Request completed successfully");
                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine($"\n[ERROR] Request timeout or cancelled for {url}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = "Request timeout or cancelled";
                    apiResponse.Status = 504; // Gateway Timeout
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"\n[ERROR] HTTP request failed for {url}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = e.Message;
                    apiResponse.Status = 500;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n[ERROR] Unexpected error for {url}");
                    Console.WriteLine($"Exception Type: {e.GetType().Name}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = $"Error: {e.Message}";
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
                    Console.WriteLine($"POST {url}: Making HTTP request...");
                    
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    apiResponse.Status = (int)response.StatusCode;
                    apiResponse.Response = responseBody;

                    cache.SetRouteCache(cacheKey, apiResponse);
                    Console.WriteLine($"POST {url}: Request completed successfully");
                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine($"\n[ERROR] Request timeout or cancelled for POST {url}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = "Request timeout or cancelled";
                    apiResponse.Status = 504; // Gateway Timeout
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"\n[ERROR] HTTP request failed for POST {url}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = e.Message;
                    apiResponse.Status = 500;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n[ERROR] Unexpected error for POST {url}");
                    Console.WriteLine($"Exception Type: {e.GetType().Name}");
                    Console.WriteLine($"Message: {e.Message}");
                    apiResponse.Response = $"Error: {e.Message}";
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
