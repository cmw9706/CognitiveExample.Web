using CognitiveExample.Web.Models.CognitiveEntities;
using CognitiveExample.Web.Services.Abstractions;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Services
{
    public class TextAnalysisService : ITextAnalysis
    {
        private ITextAnalyticsAPI _textAnalyticsAPI;
        private IList<MultiLanguageInput> _multiLanguageInput;
        private IList<AnalysisResult> _analysisResults;
        private const string _language = "en";

        public TextAnalysisService(TextAnalyticsAPI textAnalyticsAPI, IList<MultiLanguageInput> multiLanguageInput, IList<AnalysisResult> analysisResults)
        {
            _textAnalyticsAPI = textAnalyticsAPI;
            _multiLanguageInput = multiLanguageInput;
            _analysisResults = analysisResults;
        }

        public IEnumerable<AnalysisResult> AnalyzeTweets(IEnumerable<string> tweets)
        {
            for(int i = 0;i < tweets.Count(); i++)
            {
                _multiLanguageInput.Add(new MultiLanguageInput
                {
                    Language = _language,
                    Id = i.ToString(),
                    Text = tweets.ToList()[i]
                });
            }

            MultiLanguageBatchInput batchInput = new MultiLanguageBatchInput(_multiLanguageInput);

            var analysisResults = GetAnalysisResultsAsync(batchInput).Result;

            return analysisResults;
        }

        private async Task<IEnumerable<AnalysisResult>> GetAnalysisResultsAsync(MultiLanguageBatchInput batchInput)
        {
            KeyPhraseBatchResult keyPhraseBatchResult = await _textAnalyticsAPI.KeyPhrasesAsync(batchInput);
            SentimentBatchResult sentimentBatchResult = await _textAnalyticsAPI.SentimentAsync(batchInput);


            foreach(var keyPhrase in keyPhraseBatchResult.Documents)
            {
                string tweet = batchInput.Documents
                    .Where(d => d.Id == keyPhrase.Id)
                    .Select(t => t.Text)
                    .FirstOrDefault();

                var sentiment = sentimentBatchResult.Documents
                    .Where(s => s.Id == keyPhrase.Id)
                    .FirstOrDefault();

                if (IsSignificant(sentiment.Score) && !keyPhrase.KeyPhrases.Count().Equals(0))
                {
                    AnalysisResult analysisResult = new AnalysisResult
                    {
                        KeyPhrases =keyPhrase.KeyPhrases,
                        Attitude = DeriveAttitude(sentiment.Score),
                        Tweet = tweet
                    };

                    _analysisResults.Add(analysisResult);
                }
            }

            return _analysisResults;
        }

        private bool IsSignificant(double? score)
        {
            if(score > .85 || score < .20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Attitude DeriveAttitude(double? sentimentScore)
        {
            if (sentimentScore > .85)
            {
                return Attitude.Positive;
            }
            else
            {
                return Attitude.Negative;
            }
        }
    }
}
