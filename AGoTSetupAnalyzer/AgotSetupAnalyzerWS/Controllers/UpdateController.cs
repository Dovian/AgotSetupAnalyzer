using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AgotSetupAnalyzerWS.Controllers
{
    public class UpdateController : ApiController
    {
        private readonly IDatabaseProvider localDbProvider;

        public UpdateController(IDatabaseProvider localDbProvider)
        {
            this.localDbProvider = localDbProvider;
        }

        [HttpGet]
        public async Task<string> UpdateDataPull(int SetCode)
        {
            var result = await this.localDbProvider.UpdateDBBySet(SetCode);
            return result;
        }

        [HttpGet]
        public async Task<string> InitialDataPull()
        {
            var result = await this.localDbProvider.InitialDBPopulation();
            return result;
        }

        [HttpGet]
        public void LogMessage(string message)
        {
            this.localDbProvider.LogMessage(message);
        }
    }
}
