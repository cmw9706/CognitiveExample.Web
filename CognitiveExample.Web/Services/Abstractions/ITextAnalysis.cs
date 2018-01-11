using CognitiveExample.Web.Models.CognitiveEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Services.Abstractions
{
    public interface ITextAnalysis
    {
        IEnumerable<Feelings> AnalyzeTweets(IEnumerable<string> tweets);
    }
}
