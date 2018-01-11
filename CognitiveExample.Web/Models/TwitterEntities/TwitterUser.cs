using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.TwitterEntities
{
    public class TwitterUser
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("screen_name")]
        public string Username { get; set; }
        [JsonProperty("profile_image_url")]
        public string ProfilePictureUrl { get; set; }
    }
}
