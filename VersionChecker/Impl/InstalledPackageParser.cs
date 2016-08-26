using System.Collections.Generic;
using System.Web.Hosting;
using EPiServer.ServiceLocation;
using NuGet;
using VersionChecker.Interface;
using VersionChecker.Models;

namespace VersionChecker.Impl
{
    [ServiceConfiguration(typeof(IInstalledPackageParser))]
    public class InstalledPackageParser : IInstalledPackageParser
    {
        private readonly INugetFeedParser _nugetFeedParser;

        public InstalledPackageParser(INugetFeedParser nugetFeedParser)
        {
            _nugetFeedParser = nugetFeedParser;
        }

        public IList<NugetInstalledPackage> GetInstalledPackages()
        {
            var packageList = new List<NugetInstalledPackage>();
            try
            {
                var fileName = HostingEnvironment.MapPath(@"~/packages.config");
                var file = new PackageReferenceFile(fileName);

                foreach (var packageReference in file.GetPackageReferences())
                {
                    try
                    {
                        var package = _nugetFeedParser.GetCurrentFeedVersion(packageReference.Id);
                        if (package != null)
                        {
                            // OK the package is available on the Episerver Nuget feed
                            packageList.Add(new NugetInstalledPackage()
                            {
                                InstalledPackage = new NugetPackage()
                                {
                                    Id = packageReference.Id,
                                    Version = packageReference.Version.Version.ToString(),
                                    FullSemanticVersion = packageReference.Version
                                },
                                FeedPackage = package
                            });
                        }
                    }
                    catch
                    {
                        // ignored, if something goes wrong we don't want the UI to break
                    }
                }
            }
            catch
            {
                // ignored, if something goes wrong we don't want the UI to break
            }

            return packageList;
        }
    }
}
