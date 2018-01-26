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
            var model = new QueryUserViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Mentions(QueryUserViewModel queryUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new AnalysisViewModel
                {
                    Username = queryUserViewModel.TwitterHandle
                };

                var tweets = _twitterService.GetTweetsAsync(queryUserViewModel.TwitterHandle).Result;

                try
                {
                    model.AnalysisResults = _textAnalysis.AnalyzeTweets(tweets);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View(model);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}