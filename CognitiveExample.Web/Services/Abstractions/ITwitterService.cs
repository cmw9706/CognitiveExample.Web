using CognitiveExample.Web.Models.TwitterEntities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Services.Abstractions
{
    public interface ITwitterService
    {
        void GetAuthToken();
        IEnumerable<string> GetTweetsByUser(string username);
        IEnumerable<string> GetMentionsByUser(string username);
        TwitterUser GetUserInformation(string username);
    }
}
