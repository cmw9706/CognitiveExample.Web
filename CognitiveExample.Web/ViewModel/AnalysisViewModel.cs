using CognitiveExample.Web.Models.CognitiveEntities;
using CognitiveExample.Web.Models.TwitterEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.ViewModel
{
    public class AnalysisViewModel
    {
        public string Username { get; set; }
        public IEnumerable<AnalysisResult> AnalysisResults { get; set; }
    }
}
