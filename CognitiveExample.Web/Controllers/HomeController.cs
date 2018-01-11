using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.ViewModel;
using CognitiveExample.Web.Models.CognitiveEntities;

namespace CognitiveExample.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITwitterService _twitterService;
        private ITextAnalysis _textAnalysis;

        public HomeController(ITwitterService twitterService, ITextAnalysis textAnalysis)
        {
            _twitterService = twitterService;
            _textAnalysis = textAnalysis;
        }
        public IActionResult Index()
        {
            _twitterService.GetAuthToken();
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Analysis(IndexViewModel indexViewModel)
        {
            var user = _twitterService.GetUserInformation(indexViewModel.TwitterHandle);
            var model = new AnalysisViewModel
            {
                Name = user.Name,
                Username = user.Username,
                ProfileImageUrl = user.ProfilePictureUrl
            };

            if (!object.ReferenceEquals(null, user))
            {
                if (ModelState.IsValid)
                {
                    var tweets = _twitterService.GetTweetsByUser(user.Username);
                    if (tweets.Count() != 0)
                    {
                        model.Feelings = _textAnalysis.AnalyzeTweets(tweets);
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