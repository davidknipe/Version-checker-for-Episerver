namespace VersionChecker.Interface
{
    public interface IResources
    {
        string IconsPath { get; }
        string CssSuccess { get; }
        string CssWarning { get; }
        string CssDanger { get; }
        string ToolTipUpToDate { get; }
        string ToolTipUpdateRequired { get; }
        string ToolTipUpdateRecommended { get; }
        string ToolTipVersion { get; }
    }
}