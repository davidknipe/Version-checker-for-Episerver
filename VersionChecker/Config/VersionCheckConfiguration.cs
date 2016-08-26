using System.Collections.Generic;
using System.Configuration;
using EPiServer.ServiceLocation;
using VersionChecker.Interface;

namespace VersionChecker.Config
{
    /// <summary>
    /// Configuration for version checker
    /// </summary>
    [ServiceConfiguration(typeof(IVersionCheckConfiguration))]
    public class VersionCheckConfiguration : IVersionCheckConfiguration
    {
        private const string MinorVersionsBeforeWarningSetting = "versionchecker:MinorVersionsBeforeWarning";
        private const string MinorVersionsBeforeDangerSetting = "versionchecker:MinorVersionsBeforeDanger";
        private const string RolesToAccessSetting = "versionchecker:RolesToAccess";
        private const string IsEnabledSetting = "versionchecker:Enabled";

        /// <summary>
        /// If the minor versions are this many versions out then its considered a warning (set to 0 to check everything)
        /// </summary>
        public int MinorVersionsBeforeWarning => TryGetInt(MinorVersionsBeforeWarningSetting, 5);

        /// <summary>
        /// If the minor versions are this many versions out then its considered a danger warning (set to 0 to check everything)
        /// </summary>
        public int MinorVersionsBeforeDanger => TryGetInt(MinorVersionsBeforeDangerSetting, 10);

        /// <summary>
        /// The roles requires to access Version Check in the menu
        /// </summary>
        public IList<string> RolesToAccess => TryGetListString(RolesToAccessSetting, new List<string>() { "Administrators", "VersionChecker" });

        /// <summary>
        /// Set to false to disable VersionCheck - useful in development environments when not connected to the internet
        /// </summary>
        public bool Enabled => TryGetBool(IsEnabledSetting, true);

        private int TryGetInt(string settingName, int defaultVal)
        {
            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                int.TryParse(
                    ConfigurationManager.AppSettings[settingName],
                    out defaultVal);
            }
            return defaultVal;
        }

        private IList<string> TryGetListString(string settingName, List<string> defaultVal)
        {
            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                string value = ConfigurationManager.AppSettings[settingName];
                return new List<string>(value.Split(','));
            }
            return defaultVal;
        }

        private bool TryGetBool(string settingName, bool defaultVal)
        {
            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                bool.TryParse(
                    ConfigurationManager.AppSettings[settingName],
                    out defaultVal);
            }
            return defaultVal;
        }

    }
}
