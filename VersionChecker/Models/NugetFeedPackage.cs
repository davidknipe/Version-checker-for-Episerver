using System;
using NuGet;

namespace VersionChecker.Models
{
    /// <summary>
    /// Details of the Nuget package retrieved from the Episerver Nuget feed
    /// </summary>
    public class NugetPackage
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public  SemanticVersion FullSemanticVersion { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}