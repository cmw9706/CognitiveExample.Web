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
        private ILogger<TextAnalysisService> _logger;
        private IDictionary<string, string> _tweetDictionary;
        private IList<MultiLanguageInput> _multiLanguageInput;
        private IAnalysisResults _analysisResults;
        private IList<Feelings> _feelings;
        private const string _language = "en";

        public TextAnalysisService(TextAnalyticsAPI textAnalyticsAPI, ILogger<TextAnalysisService> logger, IDictionary<string, string> tweetDictionary
            , IList<MultiLanguageInput> multiLanguageInput, IAnalysisResults analysisResults, IList<Feelings> feelings)
        {
            _textAnalyticsAPI = textAnalyticsAPI;
            _logger = logger;
            _tweetDictionary = tweetDictionary;
            _multiLanguageInput = multiLanguageInput;
            _analysisResults = analysisResults;
            _feelings = feelings;
        }

        public IEnumerable<Feelings> AnalyzeTweets(IEnumerable<string> tweets)
        {
            for(int i = 0;i < tweets.Count(); i++)
            {
                _multiLanguageInput.Add(new MultiLanguageInput {Language = _language, Id = i.ToString(),Text = tweets.ToList()[i] });
                _tweetDictionary.Add(i.ToString(), tweets.ToList()[i]);
            }
            MultiLanguageBatchInput batchInput = new MultiLanguageBatchInput(_multiLanguageInput);

            return GetFeelings(batchInput).Result;
        }

        private async Task<IEnumerable<Feelings>> GetFeelings(MultiLanguageBatchInput batchInput)
        {
            try
            {
                _analysisResults.KeyPhrases =  await _textAnalyticsAPI.KeyPhrasesAsync(batchInput);
                _analysisResults.Sentiments = await _textAnalyticsAPI.SentimentAsync(batchInput);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            foreach(var keyPhrase in _analysisResults.KeyPhrases.Documents)
            {
                string tweet = string.Empty;
                var sentiment = _analysisResults.Sentiments.Documents.Where(s => s.Id == keyPhrase.Id).FirstOrDefault();
                if(_tweetDictionary.TryGetValue(keyPhrase.Id, out string value))
                {
                    tweet = value;
                }
                else
                {
                    throw new Exception("Could not map tweet to output id");
                }

                if (IsSignificant(sentiment.Score) && !keyPhrase.KeyPhrases.Count().Equals(0))
                {
                    _feelings.Add(Feelings.GetFeelings(keyPhrase.KeyPhrases, DeriveAttitude(sentiment.Score), tweet));
                }
            }

            return _feelings;
        }

        private bool IsSignificant(double? score)
        {
            if(score > .90 || score < .10)
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
            if (sentimentScore > .90)
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
