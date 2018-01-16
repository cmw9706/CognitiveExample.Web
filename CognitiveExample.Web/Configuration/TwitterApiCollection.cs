namespace CognitiveExample.Web.Configuration
{
    public class TwitterApiCollection
    {
        public string UserTimelineGetEndpoint { get; set; }
        public string UserInformationEndpoint { get; set; }
        public string SearchHandlesEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string ConsumerApiKey { get; set; }
        public string ConsumerApiSecret { get; set; }
        public string EncodedCredentials { get; set; }
        public string AuthenticationRequestBody { get; set; }
        public string PostContentType { get; set; }
        public string TweetGetCount { get; set; }
        public bool IncludeRetweets { get; set; }
        public bool ExcludeReplies { get; set; }
    }
}