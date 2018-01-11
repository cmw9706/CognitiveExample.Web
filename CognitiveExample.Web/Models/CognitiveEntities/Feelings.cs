using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public class Feelings
    {
        public ICollection<string> ThingFeltFor { get; set; }
        public Attitude AttitudeTowards { get; set; }
    }
}
