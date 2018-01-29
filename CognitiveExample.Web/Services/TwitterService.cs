using CognitiveExample.Web.Configuration;
using CognitiveExample.Web.Models.TwitterEntities;
using CognitiveExample.Web.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace CognitiveExample.Web.Services
{
    public class TwitterService : ITwitterService
    {
        private TwitterApiCollection _apiOptions;

        public TwitterService(IOptions<TwitterApiCollection> apiOptions)
        {
            _apiOptions = apiOptions.Value;
        }

        public async Task<IEnumerable<string>> GetTweetsAsync(string username)
        {
            try
            {
                OAuth2Token tokens = await GetToken();
                var tweets = await tokens.Search.TweetsAsync(new { q = username, count = _apiOptions.TweetGetCount });
                return ExtractTweetsFromSearchResults(tweets);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<string> ExtractTweetsFromSearchResults(SearchResult tweets)
        {
            var listOfValues = tweets.Select(x => x.Text);
            return listOfValues;

            //version for demo
            //List<string> ret = new List<string>();
            //foreach(var tweet in tweets)
            //{
            //    ret.Add(tweet.Text);
            //}
            //return ret;
        }

        private async Task<OAuth2Token> GetToken()
        {
            return await OAuth2.GetTokenAsync(_apiOptions.ConsumerApiKey, _apiOptions.ConsumerApiSecret);
        }

    }
}
