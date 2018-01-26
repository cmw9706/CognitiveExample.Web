using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public class AnalysisResult
    {
        public AnalysisResult()
        {

        }

        //private AnalysisResult(ICollection<string> thingFeltFor, Attitude attitude, string tweet)
        //{
        //    KeyPhrases = thingFeltFor;
        //    AttitudeTowards = attitude;
        //    Tweet = tweet;
        //}

        //public static AnalysisResult GetAnalysisResult(ICollection<string> thingFeltFor, Attitude attitude, string tweet)
        //{
        //    return new AnalysisResult(thingFeltFor, attitude, tweet);
        //}

        public ICollection<string> KeyPhrases { get; set; }
        public Attitude AttitudeTowards { get; set; }
        public string Tweet { get; set; }
    }
}
