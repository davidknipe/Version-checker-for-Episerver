namespace VersionChecker.Models
{
    /// <summary>
    /// Defines a Nuget package that is installed in the solution and available on the Episerver Nuget feed
    /// </summary>
    public class NugetInstalledPackage
    {
        /// <summary>
        /// Package detail as retrieved from the Episerver Nuget feed
        /// </summary>
        public NugetPackage FeedPackage { get; set; }

        /// <summary>
        /// Installed package detail
        /// </summary>
        public NugetPackage InstalledPackage { get; set; }
    }
}