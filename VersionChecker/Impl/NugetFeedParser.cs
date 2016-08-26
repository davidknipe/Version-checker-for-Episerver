using System;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using NuGet;
using VersionChecker.Interface;
using VersionChecker.Models;

namespace VersionChecker.Impl
{
    [ServiceConfiguration(typeof (INugetFeedParser))]
    public class NugetFeedParser : INugetFeedParser
    {
        private readonly IPackageRepository _packageRepository;
        private readonly ISynchronizedObjectInstanceCache _cache;

        public NugetFeedParser(ISynchronizedObjectInstanceCache cache)
        {
            _cache = cache;
            _packageRepository =
                PackageRepositoryFactory.Default.CreateRepository("http://nuget.episerver.com/feed/packages.svc/");
        }

        public NugetPackage GetCurrentFeedVersion(string packageId)
        {
            // We are calling out to an external service so cache results
            var cacheKey = "NugetFeedParser-Cache-" + packageId;
            if (_cache.Get("NugetFeedParser-Cache-" + packageId) != null)
            {
                var cacheVal = _cache.Get("NugetFeedParser-Cache-" + packageId);
                if (cacheVal.GetType() == typeof (NugetPackage))
                {
                    return (NugetPackage) cacheVal;
                }
                return null;
            }

            var package = _packageRepository.FindPackage(packageId);
            if (package != null)
            {
                var nugetPackage = new NugetPackage()
                {
                    Id = package.Id,
                    PublishedDate = ((NuGet.DataServicePackage) (package)).LastUpdated.DateTime,
                    Version = package.Version.Version.ToString(),
                    FullSemanticVersion = package.Version
                };
                _cache.Insert(cacheKey, nugetPackage,
                    new CacheEvictionPolicy(null, null, null, new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));
                return nugetPackage;
            }

            //We do actually want to cache the fact we could not find the package on the Episerver Nuget repo, otherwise we'd be hitting the .FindPackage method every time
            _cache.Insert(cacheKey, false,
                new CacheEvictionPolicy(null, null, null, new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));

            return null;
        }
    }
}
