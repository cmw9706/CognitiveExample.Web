using CognitiveExample.Web.Models.TwitterEntities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Services.Abstractions
{
    public interface ITwitterService
    {
        Task<IEnumerable<string>> GetTweetsAsync(string username);
    }
}
