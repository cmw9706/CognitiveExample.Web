using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.TwitterEntities
{
    public class AuthToken
    {
        public AuthToken()
        {

        }
        public AuthToken(string type, string accessToken)
        {
            Type = type;
            AccessToken = accessToken;
        }

        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
