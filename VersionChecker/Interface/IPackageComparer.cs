using VersionChecker.Impl;
using VersionChecker.Models;

namespace VersionChecker.Interface
{
    public interface IPackageComparer
    {
        Enums.PackageStatus GetPackageStatus(NugetInstalledPackage nugetInstalledPackage);
    }
}