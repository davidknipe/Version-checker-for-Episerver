using VersionChecker.Models;

namespace VersionChecker.Interface
{
    public interface INugetFeedParser
    {
        NugetPackage GetCurrentFeedVersion(string packageId);
    }
}