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
        Task<IEnumerable<string>> GetTweetsByUserAsync(string username);
        Task<IEnumerable<string>> GetMentionsByUserAsync(string username);
        TwitterUser GetUserInformation(string username);
    }
}
