using System.Collections.Generic;
using System.Linq;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Navigation;
using VersionChecker.Impl;
using VersionChecker.Interface;

namespace VersionChecker.MenuProvider
{
    [MenuProvider]
    public class VersionCheckMenu : IMenuProvider
    {
        private readonly IPackageComparer _packageComparer;
        private readonly IResources _resources;
        private readonly IVersionCheckConfiguration _versionCheckConfiguration;

        public VersionCheckMenu(IPackageComparer packageComparer, IResources resources, IVersionCheckConfiguration versionCheckConfiguration)
        {
            _packageComparer = packageComparer;
            _resources = resources;
            _versionCheckConfiguration = versionCheckConfiguration;
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            if (_versionCheckConfiguration?.Enabled == false)
                return menuItems;

            if (!HasAccessToVersionChecker())
                return menuItems;

            var mainMenuItem = new DropDownMenuItem(
                ResultMarkUp(_resources.CssSuccess),
                "/global/versionchecker")
            {
                Alignment = MenuItemAlignment.Right,
                CssClass = "",
                ToolTip = _resources.ToolTipUpToDate,
                SortIndex = int.MaxValue
            };

            var packageMenuItems = GetPackageMenuItems(mainMenuItem);
            if (packageMenuItems != null && !packageMenuItems.Any())
                return menuItems;

            menuItems.Add(mainMenuItem);
            menuItems.AddRange(packageMenuItems);

            return menuItems;
        }

        private IEnumerable<MenuItem> GetPackageMenuItems(MenuItem mainMenuItem)
        {
            var packageParser = ServiceLocator.Current.GetInstance<IInstalledPackageParser>();
            var installedPackages = packageParser.GetInstalledPackages();
            var menuItems = new List<MenuItem>();

            var hasWarning = false;
            var hasDanger = false;
            var i = 1;
            foreach (var package in installedPackages)
            {
                UrlMenuItem menuItem = null;
                var resultStyle = _resources.CssSuccess;

                var compareResult = _packageComparer.GetPackageStatus(package);
                switch (compareResult)
                {
                    case Enums.PackageStatus.UpToDate:
                        resultStyle = _resources.CssSuccess;
                        break;

                    case Enums.PackageStatus.OutOfDate:
                        resultStyle = _resources.CssWarning;
                        hasWarning = true;
                        break;

                    case Enums.PackageStatus.SeverelyOutOfDate:
                        resultStyle = _resources.CssDanger;
                        hasDanger = true;
                        break;
                }

                menuItem = new UrlMenuItem(
                    "<span>" + ResultMarkUp(resultStyle) + " <span> " + package.InstalledPackage.Id + "</span><span>",
                    "/global/versionchecker/" + i.ToString(),
                    "javascript: return;"
                    )
                {
                    ToolTip =
                        string.Format(_resources.ToolTipVersion, package.InstalledPackage.Version,
                            package.FeedPackage.Version),
                    SortIndex = i
                };

                menuItems.Add(menuItem);
                i++;
            }

            if (hasWarning)
            {
                mainMenuItem.Text = ResultMarkUp(_resources.CssWarning);
                mainMenuItem.ToolTip = _resources.ToolTipUpdateRequired;
            }
            if (hasDanger)
            {
                mainMenuItem.Text = ResultMarkUp(_resources.CssDanger);
                mainMenuItem.ToolTip = _resources.ToolTipUpdateRecommended;
            }

            return menuItems;
        }

        private string ResultMarkUp(string resultStyle) => @"<span style='" + resultStyle + "'></span>";

        private bool HasAccessToVersionChecker()
        {
            return _versionCheckConfiguration.RolesToAccess.Any(roleName => PrincipalInfo.Current.Principal.IsInRole(roleName));
        }
    }
}
