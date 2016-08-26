using System.Collections.Generic;
using VersionChecker.Models;

namespace VersionChecker.Interface
{
    public interface IInstalledPackageParser
    {
        /// <summary>
        /// Get a list of installed packages that are published on the Episerver Nuget feed
        /// </summary>
        /// <returns></returns>
        IList<NugetInstalledPackage> GetInstalledPackages();
    }
}