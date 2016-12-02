using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace AgotSetupAnalyzerWS.Jobs
{
    public class RequestSyncJob : IRegisteredObject, IRequestSyncJob
    {
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        private readonly IDatabaseProvider _localDbProvider;

        public RequestSyncJob(
            IDatabaseProvider localDbProvider)
        {
            Debug.Print("Initializing RequestSyncJob @ {0}", DateTimeOffset.Now);

            _localDbProvider = localDbProvider;
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            Debug.Print("Executing RequestSyncJob @ {0}", DateTimeOffset.Now);

            _localDbProvider.LogMessage("Starting Sync");
            //_localDbProvider.UpdateFullDb();

            Debug.Print("Completed RequestSyncJob @ {0}", DateTimeOffset.Now);
        }

        public void Stop(bool immediate)
        {
            _source.Cancel(immediate);
            HostingEnvironment.UnregisterObject(this);
        }
    }
}