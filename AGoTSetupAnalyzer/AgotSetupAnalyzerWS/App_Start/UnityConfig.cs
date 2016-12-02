using AgotSetupAnalyzer;
using AgotSetupAnalyzerCore;
using AgotSetupAnalyzerDB;
using AgotSetupAnalyzerThronesDB;
using AgotSetupAnalyzerWS.Configs;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Web.Http;
using System.Web.Mvc;
using Unity.WebApi;
using AgotSetupAnalyzerWS.Jobs;

namespace AgotSetupAnalyzerWS
{
    public static class UnityConfig
    {
        public static IUnityContainer Configure()
        {
			var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            
            RegisterServices(container);
            
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));

            return container;
        }

        private static void RegisterServices(UnityContainer container)
        {
            container.RegisterType<IDeckAnalyzer, DeckAnalyzer>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IDatabaseProvider, DatabaseProvider>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IDbReader, DBReader>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IDbWriter, DBWriter>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IThronesDBProvider, ThronesDBProvider>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            container.RegisterType<ILocalDBConfig, LocalDBConfig>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
            
            container.RegisterType<IRequestSyncJob, RequestSyncJob>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());
        }
    }
}