using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Security.Policy;

namespace Proxy
{

    public class Cache 
    {
        private static readonly TimeSpan DEFAULT_DURATION = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan CONTRACTS_DURATION = TimeSpan.FromDays(2);
        private static readonly TimeSpan STATIONS_DURATION = DEFAULT_DURATION;

        private readonly MemoryCache cache = MemoryCache.Default;

        public APIResponse GetRouteCache(string url)
        {
            APIResponse response = null;

            if (cache.Contains(url))
            {
               response = (APIResponse) cache.Get(url);
            }

            if (response != null)
            {
                Console.WriteLine($"{url} (Cached)");
            }

            return response;
        }

        public void SetRouteCache(string url, APIResponse response)
        {
            Uri uri = new Uri(url);

            string route = uri.Segments.Last().Trim('/');

            TimeSpan duration;

            switch (route)
            {
                case "contracts":
                    duration = CONTRACTS_DURATION;
                    break;

                case "stations":
                    duration = STATIONS_DURATION;
                    break;

                default:
                    duration = DEFAULT_DURATION;
                    break;
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(duration),
                RemovedCallback = args =>
                {
                    Console.WriteLine($"{args.CacheItem.Key} ({args.RemovedReason})");
                }
            };

            cache.Set(url, response, policy);
            Console.WriteLine($"{url} (Cached)");
        }
    }
}
