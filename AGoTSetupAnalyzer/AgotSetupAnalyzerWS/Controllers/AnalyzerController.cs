using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public PartialViewResult AnalyzeDeck(AnalyzerConfigurationDTO dto)
        {
            var results = analyzer.Analyze(dto);

            return PartialView(results);
        }

    }
}
