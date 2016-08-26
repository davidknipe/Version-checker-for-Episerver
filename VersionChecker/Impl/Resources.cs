using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using VersionChecker.Interface;

namespace VersionChecker.Impl
{
    [ServiceConfiguration(typeof(IResources))]
    public class Resources : IResources
    {
        private readonly LocalizationService _localizationService;

        public Resources(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public string IconsPath => Paths.ToClientResource("Shell", "ClientResources/epi/themes/sleek/epi/images/icons/notificationIcons16x16.png");
        private string IconStyle => "background: url(" + IconsPath + ") 0 -{0}px no-repeat; height: 16px; width: 16px; cursor: default;";

        public string CssSuccess => string.Format(IconStyle, "16");
        public string CssWarning => string.Format(IconStyle, "48");
        public string CssDanger => string.Format(IconStyle, "32");

        public string ToolTipUpToDate => _localizationService.GetString("/versionchecker/uptodate", "Episerver is up to date");
        public string ToolTipUpdateRequired => _localizationService.GetString("/versionchecker/updatesavailable", "Episerver updates are available");
        public string ToolTipUpdateRecommended => _localizationService.GetString("/versionchecker/updatesavailable", "Episerver updates is recommended");
        public string ToolTipVersion => _localizationService.GetString("/versionchecker/versionsavailable", "Installed version: {0}\nLatest version: {1}");
    }
}
