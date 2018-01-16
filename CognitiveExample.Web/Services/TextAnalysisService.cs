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
        private string _language = "en";
        private IDictionary<string, string> _tweetDictionary;

        public TextAnalysisService(TextAnalyticsAPI textAnalyticsAPI, ILogger<TextAnalysisService> logger, IDictionary<string, string> tweetDictionary)
        {
            _textAnalyticsAPI = textAnalyticsAPI;
            _logger = logger;
            _tweetDictionary = tweetDictionary;
        }

        public IEnumerable<Feelings> AnalyzeTweets(IEnumerable<string> tweets)
        {
            
            var tweetInputs = new List<MultiLanguageInput>();
            for(int i = 0;i < tweets.Count(); i++)
            {
                tweetInputs.Add(new MultiLanguageInput {Language = _language, Id = i.ToString(),Text = tweets.ToList()[i] });
                _tweetDictionary.Add(i.ToString(), tweets.ToList()[i]);
            }
            MultiLanguageBatchInput batchInput = new MultiLanguageBatchInput(tweetInputs);

            return GetFeelings(batchInput);
        }

        private IEnumerable<Feelings> GetFeelings(MultiLanguageBatchInput batchInput)
        {
            AnalysisResults analysis = new AnalysisResults();
            IList<Feelings> listOfFeelings = new List<Feelings>();
            try
            {
                analysis.KeyPhrases = _textAnalyticsAPI.KeyPhrases(batchInput);
                analysis.Sentiments = _textAnalyticsAPI.Sentiment(batchInput);

            }
            catch(Exception ex)
            {
                throw ex;
            }



            foreach(var keyPhrase in analysis.KeyPhrases.Documents)
            {
                string tweet = string.Empty;
                var sentiment = analysis.Sentiments.Documents.Where(s => s.Id == keyPhrase.Id).FirstOrDefault();
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
                    Feelings feeling = new Feelings
                    {
                        ThingFeltFor = keyPhrase.KeyPhrases,
                        AttitudeTowards = DeriveAttitude(sentiment.Score),
                        Tweet = tweet
                    };
                    listOfFeelings.Add(feeling);
                }
            }

            return listOfFeelings;
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
