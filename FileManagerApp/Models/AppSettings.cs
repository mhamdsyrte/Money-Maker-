namespace FileManagerApp.Models;

public class AppSettings
{
    public string Theme { get; set; } = "Dark";
    public string Language { get; set; } = "ar";
    public string AccentColor { get; set; } = "#3b82f6";
    public int FontSize { get; set; } = 14;
    public string FontFamily { get; set; } = "CairoRegular";
    public bool ShowHiddenFiles { get; set; } = false;
    public string ViewMode { get; set; } = "Grid"; // Grid or List
    public string SortBy { get; set; } = "Name"; // Name, Size, Date, Type
    public bool SortDescending { get; set; } = false;
    public List<string> FavoritePaths { get; set; } = new();
    public List<string> RecentPaths { get; set; } = new();
    public int EditorFontSize { get; set; } = 14;
    public bool EditorWordWrap { get; set; } = true;
    public bool EditorLineNumbers { get; set; } = true;
    public bool EditorSyntaxHighlight { get; set; } = true;
}

public class ThemeColors
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string SurfaceColor { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string TextPrimaryColor { get; set; } = string.Empty;
    public string TextSecondaryColor { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public string SuccessColor { get; set; } = string.Empty;
    public string WarningColor { get; set; } = string.Empty;
    public string DangerColor { get; set; } = string.Empty;
    public string BorderColor { get; set; } = string.Empty;
}
