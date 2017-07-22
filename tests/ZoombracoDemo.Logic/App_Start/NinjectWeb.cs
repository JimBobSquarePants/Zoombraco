[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ZoombracoDemo.Logic.NinjectWeb), "Start")]

namespace ZoombracoDemo.Logic
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject.Web;

    public static class NinjectWeb
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }
    }
}