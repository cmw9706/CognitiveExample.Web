using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.ViewModel;
using CognitiveExample.Web.Models.CognitiveEntities;
using Microsoft.Extensions.Logging;

namespace CognitiveExample.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        private ITwitterService _twitterService;
        private ITextAnalysis _textAnalysis;

        public HomeController(ITwitterService twitterService, ITextAnalysis textAnalysis, ILogger<HomeController> logger)
        {
            _textAnalysis = textAnalysis;
            _twitterService = twitterService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _twitterService.GetAuthToken();
            var model = new QueryUserViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Mentions(QueryUserViewModel queryUserViewModel)
        {
            var user = _twitterService.GetUserInformation(queryUserViewModel.TwitterHandle);

            if (!object.ReferenceEquals(null, user))
            {
                var model = new AnalysisViewModel
                {
                    Name = user.Name,
                    Username = user.Username,
                    ProfileImageUrl = user.ProfilePictureUrl
                };

                if (ModelState.IsValid)
                {
                    var tweets = _twitterService.GetMentionsByUserAsync(user.Username).Result;

                    if (tweets.ToList().Count() != 0)
                    {
                        try
                        {
                            model.Feelings = _textAnalysis.AnalyzeTweets(tweets);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failure to extract sentiments from text");
                            throw ex;
                        }
                    }
                    else
                    {
                        model.Feelings = new List<Feelings>();
                    }

                    return View(model);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

    }
}