using System;
using Xunit;
using CognitiveExample.Web.Configuration;
using CognitiveExample.Web.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.Models.TwitterEntities;
using CognitiveExample.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using CognitiveExample.Web.Models.CognitiveEntities;
using CognitiveExample.Web.ViewModel;

namespace CognitiveExample.Web.UnitTest.Controllers
{
    public class TwitterControllerUnitTest
    {
        private Mock<ITwitterService> _twitterServiceMock;
        private Mock<ITextAnalysis> _textAnalyticsMock;
        private TwitterController _twitterController;

        public TwitterControllerUnitTest()
        {
            var logger = new Mock<ILogger<TwitterController>>();

            _twitterServiceMock = new Mock<ITwitterService>();
            _textAnalyticsMock = new Mock<ITextAnalysis>();
            _twitterController = new TwitterController(_twitterServiceMock.Object, _textAnalyticsMock.Object, logger.Object);
        }

        [Fact]
        public void CanReturnQueryUserView()
        {
            var result = _twitterController.QueryUser();

            Assert.IsType(typeof(ViewResult), result);
        }

        [Fact]
        public void CanReturnMentions()
        {
            IEnumerable<string> tweets = new List<string>
            {
                "This is a tweet",
                "This is also a tweet",
                "This is another tweet"
            };
            TwitterUser twitterUser = new TwitterUser
            {
                Id = "asd",
                Name = "Connor",
                ProfilePictureUrl = "pic",
                Username = "connorsusername"
            };
            List<Feelings> feelings = new List<Feelings>
            {
                Feelings.GetFeelings(new List<string>{ "thing " },Attitude.Positive, "This is a tweet"),
                Feelings.GetFeelings(new List<string>{ "thing " },Attitude.Positive, "This is also a tweet"),
                Feelings.GetFeelings(new List<string>{ "thing " },Attitude.Positive, "This is another tweet"),
            };

            QueryUserViewModel queryUserViewModel = new QueryUserViewModel
            {
                TwitterHandle = "twitterhandle"
            };

            Mock<QueryUserViewModel> viewModelMock = new Mock<QueryUserViewModel>();

            _twitterServiceMock.Setup(service => service.GetUserInformation(It.IsAny<string>())).Returns(twitterUser);
            _twitterServiceMock.Setup(service => service.GetMentionsByUserAsync(It.IsAny<string>())).ReturnsAsync(tweets);
            _textAnalyticsMock.Setup(analyzeService => analyzeService.AnalyzeTweets(It.IsAny<List<string>>())).Returns(feelings);

            var result = _twitterController.Mentions(viewModelMock.Object);


            Assert.IsType(typeof(ViewResult), result);
        }
    }
}
