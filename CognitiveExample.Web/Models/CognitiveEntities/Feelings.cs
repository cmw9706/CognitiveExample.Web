using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public class Feelings
    {
        private Feelings(ICollection<string> thingFeltFor, Attitude attitude, string tweet)
        {
            ThingFeltFor = thingFeltFor;
            AttitudeTowards = attitude;
            Tweet = tweet;
        }

        public static Feelings GetFeelings(ICollection<string> thingFeltFor, Attitude attitude, string tweet)
        {
            return new Feelings(thingFeltFor, attitude, tweet);
        }

        public ICollection<string> ThingFeltFor { get; set; }
        public Attitude AttitudeTowards { get; set; }
        public string Tweet { get; set; }
    }
}
