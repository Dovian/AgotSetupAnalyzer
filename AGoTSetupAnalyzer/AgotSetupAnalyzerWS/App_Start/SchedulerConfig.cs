using AgotSetupAnalyzerWS.Jobs;
using FluentScheduler;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerWS
{
    public class SchedulerConfig
    {
        public static void Configure(IUnityContainer container)
        {
            JobManager.JobFactory = new UnityJobFactory(container);
            JobManager.Initialize(new SchedulerRegistry());
        }
    }

    class UnityJobFactory : IJobFactory
    {
        private readonly IUnityContainer _container;

        public UnityJobFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IJob GetJobInstance<T>() where T : IJob
        {
            return _container.Resolve<T>();
        }
    }

    class SchedulerRegistry : Registry
    {
        public SchedulerRegistry()
        {
            Schedule<IRequestSyncJob>().NonReentrant().ToRunEvery(24).Hours();
        }
    }
}