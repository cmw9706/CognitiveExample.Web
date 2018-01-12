using CognitiveExample.Web.Models.CognitiveEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveExample.Web.ViewModel
{
    public class AnalysisViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string ProfileImageUrl { get; set; }
        public IEnumerable<Feelings> Feelings { get; set; }
    }
}
