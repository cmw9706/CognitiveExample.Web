using System;
using Xunit;
using CognitiveExample.Web.Configuration;
using CognitiveExample.Web.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.Models.TwitterEntities;

namespace CognitiveExample.Web.UnitTest.Services
{
    public class TwitterServiceUnitTest
    {
        private TwitterService _twitterService;
        private TwitterApiCollection _apiCollection;

        public TwitterServiceUnitTest()
        {
            var logger = new Mock<ILogger<TwitterService>>();
            _apiCollection = new TwitterApiCollection
            {
                TokenEndpoint = "https://api.twitter.com/oauth2/token",
                ConsumerApiKey = "iTQBPk2bYJvyFwC6BWGT38jSy",
                ConsumerApiSecret = "RYld6Et7T9n7MFP3QXKhVR1YjvHHnUnZAv9Ggsxu96oDHX79CA"
            };
            _twitterService = new TwitterService(Options.Create(_apiCollection));
        }
    }
}
