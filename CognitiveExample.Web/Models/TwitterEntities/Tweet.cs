using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.TwitterEntities
{
    public class Tweet
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("favorite_count")]
        public int FavoriteCount { get; set; }
        [JsonProperty("retweet_count")]
        public int RetweetCount { get; set; }
    }
}
