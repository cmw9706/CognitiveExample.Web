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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Getting home page.");
            return View();
        }
    }
}