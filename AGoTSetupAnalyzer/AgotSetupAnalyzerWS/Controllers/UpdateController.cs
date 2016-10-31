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
        public async Task<IEnumerable<string>> UpdateDataPull(int SetCode)
        {
            var result = await this.localDbProvider.UpdateDB(SetCode);
            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> InitialDataPull()
        {
            var result = await this.localDbProvider.InitialDBPopulation();
            return result;
        }
    }
}
