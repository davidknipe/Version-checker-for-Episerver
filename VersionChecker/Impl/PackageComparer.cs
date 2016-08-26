using EPiServer.ServiceLocation;
using VersionChecker.Interface;
using VersionChecker.Models;

namespace VersionChecker.Impl
{
    [ServiceConfiguration(typeof(IPackageComparer))]
    public class PackageComparer : IPackageComparer
    {
        private readonly IVersionCheckConfiguration _versionCheckConfiguration;

        public PackageComparer(IVersionCheckConfiguration versionCheckConfiguration)
        {
            _versionCheckConfiguration = versionCheckConfiguration;
        }

        public Enums.PackageStatus GetPackageStatus(NugetInstalledPackage nugetInstalledPackage)
        {
            // Same version means up to date
            if (nugetInstalledPackage.FeedPackage.Version ==
                nugetInstalledPackage.InstalledPackage.Version)
            {
                return Enums.PackageStatus.UpToDate;
            }

            // If major versions are out of date, it will produce a danger warning
            if (nugetInstalledPackage.FeedPackage.FullSemanticVersion.Version.Major !=
                nugetInstalledPackage.InstalledPackage.FullSemanticVersion.Version.Major)
            {
                return Enums.PackageStatus.SeverelyOutOfDate;
            }

            // Check how many minor versions out of date for a danger warning
            if (nugetInstalledPackage.FeedPackage.FullSemanticVersion.Version.Minor -
                nugetInstalledPackage.InstalledPackage.FullSemanticVersion.Version.Minor > _versionCheckConfiguration.MinorVersionsBeforeDanger)
            {
                return Enums.PackageStatus.SeverelyOutOfDate;
            }

            // Check how many minor versions out of date for a warning
            if (nugetInstalledPackage.FeedPackage.FullSemanticVersion.Version.Minor -
                nugetInstalledPackage.InstalledPackage.FullSemanticVersion.Version.Minor > _versionCheckConfiguration.MinorVersionsBeforeWarning)
            {
                return Enums.PackageStatus.OutOfDate;
            }

            return Enums.PackageStatus.UpToDate;
        }
    }
}
