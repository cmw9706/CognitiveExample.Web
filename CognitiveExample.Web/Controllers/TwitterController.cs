using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CognitiveExample.Web.ViewModel;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.Models.CognitiveEntities;
using System.Net;
using Microsoft.Extensions.Logging;

namespace CognitiveExample.Web.Controllers
{
    public class TwitterController : Controller
    {
        private ITwitterService _twitterService;
        private ITextAnalysis _textAnalysis;
        private ILogger<TwitterController> _logger;

        public TwitterController(ITwitterService twitterService, ITextAnalysis textAnalysis, ILogger<TwitterController> logger)
        {
            _twitterService = twitterService;
            _textAnalysis = textAnalysis;
            _logger = logger;
        }

        public IActionResult QueryUser()
        {
            _twitterService.GetAuthToken();
            var model = new QueryUserViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Analysis(QueryUserViewModel queryUserViewModel)
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
                    var tweets = _twitterService.GetTweetsByUserAsync(user.Username).Result;

                    if (tweets.ToList().Count() != 0)
                    {
                        try
                        {
                            model.Feelings = _textAnalysis.AnalyzeTweets(tweets);
                        }
                        catch(Exception ex)
                        {
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