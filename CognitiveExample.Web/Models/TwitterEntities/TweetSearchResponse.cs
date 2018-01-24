using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.TwitterEntities
{
    [JsonObject]
    public class TweetSearchResponse
    {
        [JsonProperty("statuses")]
        public Tweet[] Tweets { get; set; }
    }
}
