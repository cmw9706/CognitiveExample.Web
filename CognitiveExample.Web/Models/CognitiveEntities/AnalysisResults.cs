using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public class AnalysisResults
    {
        public KeyPhraseBatchResult KeyPhrases { get; set; }
        public SentimentBatchResult Sentiments { get; set; }
    }
}
