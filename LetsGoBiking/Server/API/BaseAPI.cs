using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.API
{
    public class BaseAPI
    {
        protected readonly ProxyServiceClient client = new ProxyServiceClient();

        protected string LoadApiKey(string key)
        {
            var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            if (File.Exists(envPath))
            {
                var lines = File.ReadAllLines(envPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key+"="))
                    {
                        return line.Substring(key.Length + 1).Trim();
                    }
                }
            }
            throw new Exception(key + " not found in .env file");
        }
    }
}
