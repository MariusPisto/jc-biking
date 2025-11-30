using Newtonsoft.Json;
using System.Collections.Generic;

namespace Server.Entities.Dott
{
    public class DottFeedResponse
    {
        public DottFeedData data { get; set; }
    }

    public class DottFeedData
    {
        public DottFeedLanguage en { get; set; }
    }

    public class DottFeedLanguage
    {
        public List<DottFeed> feeds { get; set; }
    }

    public class DottFeed
    {
        public string name { get; set; }
        public string url { get; set; }
    }
}
