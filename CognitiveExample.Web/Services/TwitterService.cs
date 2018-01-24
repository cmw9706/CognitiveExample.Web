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

        public async Task<IEnumerable<string>> GetMentionsByUserAsync(string username)
        {
            
            HttpWebRequest getMentions = CreateMentionsGet(username);
            try
            {
                List<string> listOfTweets = new List<string>();
                var response = await ExecuteGetListOfTweetsAsync(getMentions);
                var tweets = JsonConvert.DeserializeObject<TweetSearchResponse>(response);
                foreach(var tweet in tweets.Tweets)
                {
                    listOfTweets.Add(tweet.Text);
                }
                return listOfTweets;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        private static async Task<string> ExecuteGetListOfTweetsAsync(HttpWebRequest getList)
        {
            
            using (HttpWebResponse response = (HttpWebResponse)await getList.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
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

        private HttpWebRequest CreateMentionsGet(string username)
        {
            var getRequest = WebRequest.Create(_apiOptions.SearchHandlesEndpoint + "?q=@" + username + "&count=" + _apiOptions.TweetGetCount) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + _authToken.AccessToken;
            return getRequest;
        }

        private HttpWebRequest CreateUserInfoGet(string username)
        {
            var getRequest = WebRequest.Create(_apiOptions.UserInformationEndpoint + "?screen_name=" + username) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + _authToken.AccessToken;
            return getRequest;
        }

        private HttpWebRequest CreateAuthPost()
        {
            var post = WebRequest.Create(_apiOptions.TokenEndpoint) as HttpWebRequest;
            post.Method = _httpPost;
            post.ContentType = _apiOptions.PostContentType;
            post.Headers[HttpRequestHeader.Authorization] = "Basic " +Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes( _apiOptions.ConsumerApiKey+":"+_apiOptions.ConsumerApiSecret));
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
