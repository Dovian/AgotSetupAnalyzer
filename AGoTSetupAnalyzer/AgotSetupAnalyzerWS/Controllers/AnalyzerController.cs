using AgotSetupAnalyzerCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AgotSetupAnalyzerWS.Controllers
{
    public class AnalyzerController : Controller
    {
        private readonly IDeckAnalyzer analyzer;

        public AnalyzerController(IDeckAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> AnalyzeDeck(AnalyzerConfigurationDTO dto)
        {
            var results = await analyzer.Analyze(dto);

            return JsonConvert.SerializeObject(results);
        }

    }
}
