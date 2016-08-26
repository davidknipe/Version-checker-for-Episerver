using System.Collections;
using System.Collections.Generic;

namespace VersionChecker.Interface
{
    /// <summary>
    /// Configuration for version checker
    /// </summary>
    public interface IVersionCheckConfiguration
    {
        /// <summary>
        /// If the minor versions are this many versions out then its considered a danger warning (set to 0 not to check)
        /// </summary>
        int MinorVersionsBeforeWarning { get; }

        /// <summary>
        /// If the minor versions are this many versions out then its considered a danger warning (set to 0 not to check)
        /// </summary>
        int MinorVersionsBeforeDanger { get; }

        /// <summary>
        /// The roles requires to access Version Check in the menu
        /// </summary>
        IList<string> RolesToAccess { get; }

        /// <summary>
        /// Set to false to disable VersionCheck - useful in development environments when not connected to the internet
        /// </summary>
        bool Enabled { get; }
    }
}