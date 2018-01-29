using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.Models.CognitiveEntities
{
    public class AnalysisResult
    {
        public IEnumerable<string> KeyPhrases { get; set; }
        public Attitude Attitude { get; set; }
        public string Tweet { get; set; }
    }
}
