using AgotSetupAnalyzerCore;
using AgotSetupAnalyzerDB;
using AgotSetupAnalyzerThronesDB;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace AgotSetupAnalyzerWS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IDatabaseProvider, DatabaseProvider>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IDbReader, DBReader>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IDbWriter, DBWriter>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IThronesDBProvider, ThronesDBProvider>(
                new ContainerControlledLifetimeManager());
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}