using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public interface IAnalysisResults
    {
        KeyPhraseBatchResult KeyPhrases { get; set; }
        SentimentBatchResult Sentiments { get; set; }
    }
}