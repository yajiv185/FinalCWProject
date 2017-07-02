using Cache;
using Entities;
using Interfaces;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace Services
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<IDataAccess<ReadStock>, CacheLayer>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}