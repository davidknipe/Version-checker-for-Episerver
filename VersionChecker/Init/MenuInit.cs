using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using VersionChecker.Interface;

namespace VersionChecker.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class MenuInit : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var config = ServiceLocator.Current.GetInstance<IVersionCheckConfiguration>();
            if (config?.Enabled ==false)
                return;

            // Initialise the cache of the packages here, if done on the first request 
            // to the menu in the UI it can affect the ordering of menu items
            var initMe = ServiceLocator.Current.GetInstance<IInstalledPackageParser>();
            initMe?.GetInstalledPackages(); // Warms up the cache
        }

        public void Uninitialize(InitializationEngine context) { }
    }
}