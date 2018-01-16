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

namespace CognitiveExample.Web.Services
{
    public class TwitterService : ITwitterService
    {
        private TwitterApiCollection _apiOptions;
        private ILogger<TwitterService> _logger;
        private AuthToken _authToken;
        private const string _httpGet = "GET";
        private const string _httpPost = "POST";

        public TwitterService(IOptions<TwitterApiCollection> apiOptions, ILogger<TwitterService> logger)
        {
            _apiOptions = apiOptions.Value;
            _logger = logger;
        }

        //TODO: Move this to startup - when singleton instance is built
        public void GetAuthToken()
        {
            HttpWebRequest postBearer = CreateAuthPost();
            try
            {
                string responseBody = string.Empty;
                using (var response = postBearer.GetResponse().GetResponseStream())
                {
                    var responseReader = new StreamReader(response);
                    responseBody = responseReader.ReadToEnd();
                }

                _authToken = JsonConvert.DeserializeObject<AuthToken>(responseBody);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public IEnumerable<string> GetTweetsByUser(string username)
        {
            List<string> listOfTweets = new List<string>();
            HttpWebRequest getTimeline = CreateTweetsGet(username);
            try
            {
                string responseBody = string.Empty;
                using (var response = getTimeline.GetResponse().GetResponseStream())//there request sends
                {
                    var responseReader = new StreamReader(response);
                    responseBody = responseReader.ReadToEnd();
                }

                IList<Tweet> tweets = JsonConvert.DeserializeObject<List<Tweet>>(responseBody);

                foreach (var tweet in tweets)
                {
                    listOfTweets.Add(tweet.Text);
                }
            }
            catch(Exception ex) //401 (access token invalid or expired)
            {
                //TODO
            }

            return listOfTweets;
        }

        public IEnumerable<string> GetMentionsByUser(string username)
        {
            List<string> listOfTweets = new List<string>();
            HttpWebRequest getMentions = CreateMentionsGet(username);
            try
            {
                string responseBody = string.Empty;
                using (var response = getMentions.GetResponse().GetResponseStream())//there request sends
                {
                    var responseReader = new StreamReader(response);
                    responseBody = responseReader.ReadToEnd();
                }

                MentionTweets tweets = JsonConvert.DeserializeObject<MentionTweets>(responseBody);

                foreach (var tweet in tweets.Tweets)
                {
                    listOfTweets.Add(tweet.Text);
                }
            }
            catch (Exception ex) //401 (access token invalid or expired)
            {
                //TODO
            }

            return listOfTweets;
        }

        private HttpWebRequest CreateMentionsGet(string username)
        {
            var getRequest = WebRequest.Create(_apiOptions.SearchHandlesEndpoint + "?q=@" + username + "&count=" + _apiOptions.TweetGetCount) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + _authToken.AccessToken;
            return getRequest;
        }

        public TwitterUser GetUserInformation(string username)
        {
            HttpWebRequest getUserInfo = CreateUserInfoGet(username);
            try
            {
                string responseBody = string.Empty;
                using (var response = getUserInfo.GetResponse().GetResponseStream())//there request sends
                {
                    var responseReader = new StreamReader(response);
                    responseBody = responseReader.ReadToEnd();
                }

                var twitterUser = JsonConvert.DeserializeObject<TwitterUser>(responseBody);
                return twitterUser;
            }
            catch (Exception ex) //401 (access token invalid or expired)
            {
                //add 404 handling
                throw ex;
            }
        }

        private HttpWebRequest CreateUserInfoGet(string username)
        {
            var getRequest = WebRequest.Create(_apiOptions.UserInformationEndpoint + "?screen_name=" + username) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + _authToken.AccessToken;
            return getRequest;
        }

        private HttpWebRequest CreateTweetsGet(string username)
        {
            var getRequest = WebRequest.Create(_apiOptions.UserTimelineGetEndpoint + "?count="+_apiOptions.TweetGetCount+"&screen_name="+username+"&include_rts="+_apiOptions.IncludeRetweets+ "exclude_replies="+_apiOptions.ExcludeReplies) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + _authToken.AccessToken;
            return getRequest;
        }

        private HttpWebRequest CreateAuthPost()
        {
            var post = WebRequest.Create(_apiOptions.TokenEndpoint) as HttpWebRequest;
            post.Method = _httpPost;
            post.ContentType = _apiOptions.PostContentType;

            post.Headers[HttpRequestHeader.Authorization] = "Basic " + _apiOptions.EncodedCredentials;
            var reqbody = Encoding.UTF8.GetBytes(_apiOptions.AuthenticationRequestBody);
            post.ContentLength = reqbody.Length;

            using (var req = post.GetRequestStream())
            {
                req.Write(reqbody, 0, reqbody.Length);
            }

            return post;
        }
    }
}
