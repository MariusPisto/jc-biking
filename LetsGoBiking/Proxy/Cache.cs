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
        private static readonly CacheItemPolicy DEFAULT_POLICY = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(10),
        };

        private static readonly CacheItemPolicy CONTRACTS_POLICY = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(2),
        };

        private static readonly CacheItemPolicy STATIONS_POLICY = DEFAULT_POLICY;
        private readonly MemoryCache cache = MemoryCache.Default;

        public APIResponse GetRouteCache(string url)
        {
            APIResponse response = null;

            if (cache.Contains(url))
            {
               response = (APIResponse) cache.Get(url);
            }

            Console.WriteLine(url + " is cached: " + (response != null));

            return response;
        }

        public void SetRouteCache(string url, APIResponse response)
        {
            Uri uri = new Uri(url);

            string route = uri.Segments.Last().Trim('/');

            CacheItemPolicy policy;

            switch (route)
            {
                case "contracts":
                    policy = CONTRACTS_POLICY;
                    break;

                case "stations":
                    policy = STATIONS_POLICY;
                    break;

                default:
                    policy = DEFAULT_POLICY;
                    break;
            }

            cache.Set(url, response, policy);
        }
    }
}
