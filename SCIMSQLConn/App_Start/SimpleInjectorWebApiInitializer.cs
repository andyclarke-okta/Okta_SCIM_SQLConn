[assembly: WebActivator.PostApplicationStartMethod(typeof(SCIMSQLConn.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace SCIMSQLConn.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
   // using Okta.SCIM.Server.Connectors;
    using SCIMSQLConn.Connectors;
    
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: https://bit.ly/YE8OJj.
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            // This is where you register your connector with the IOC container
            //container.RegisterWebApiRequest<ISCIMConnector, InMemorySCIMConnector>();
            container.RegisterWebApiRequest<ISCIMConnector, SQLServerSCIMConnector>();
            //container.RegisterWebApiRequest<ISCIMConnector, SfdcProvConnector>();
        }
    }
}