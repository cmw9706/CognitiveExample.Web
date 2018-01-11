using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace CognitiveExample.Web.Configuration
{
    public class TextAnalyticsApiConfig
    {
        public AzureRegions AzureRegion { get; set; }
        public string SubscriptionKey { get; set; }
    }
}