using FileManagerApp.Models;

namespace FileManagerApp.Services;

public class ThemeService
{
    public List<ThemeColors> AvailableThemes { get; } = new()
    {
        new ThemeColors
        {
            Name = "Dark",
            DisplayName = "داكن",
            Icon = "🌙",
            BackgroundColor = "#0a0e1a",
            SurfaceColor = "#1e293b",
            PrimaryColor = "#3b82f6",
            SecondaryColor = "#8b5cf6",
            TextPrimaryColor = "#f8fafc",
            TextSecondaryColor = "#94a3b8",
            AccentColor = "#3b82f6",
            SuccessColor = "#22c55e",
            WarningColor = "#f59e0b",
            DangerColor = "#ef4444",
            BorderColor = "#334155"
        },
        new ThemeColors
        {
            Name = "Light",
            DisplayName = "فاتح",
            Icon = "☀️",
            BackgroundColor = "#f8fafc",
            SurfaceColor = "#ffffff",
            PrimaryColor = "#3b82f6",
            SecondaryColor = "#8b5cf6",
            TextPrimaryColor = "#0f172a",
            TextSecondaryColor = "#64748b",
            AccentColor = "#3b82f6",
            SuccessColor = "#16a34a",
            WarningColor = "#d97706",
            DangerColor = "#dc2626",
            BorderColor = "#e2e8f0"
        },
        new ThemeColors
        {
            Name = "Midnight",
            DisplayName = "منتصف الليل",
            Icon = "🌌",
            BackgroundColor = "#0a0a1a",
            SurfaceColor = "#151530",
            PrimaryColor = "#6366f1",
            SecondaryColor = "#a855f7",
            TextPrimaryColor = "#e0e0ff",
            TextSecondaryColor = "#8080a0",
            AccentColor = "#6366f1",
            SuccessColor = "#34d399",
            WarningColor = "#fbbf24",
            DangerColor = "#f87171",
            BorderColor = "#2a2a50"
        },
        new ThemeColors
        {
            Name = "Forest",
            DisplayName = "غابة",
            Icon = "🌲",
            BackgroundColor = "#0a1f0a",
            SurfaceColor = "#0f2f0f",
            PrimaryColor = "#22c55e",
            SecondaryColor = "#84cc16",
            TextPrimaryColor = "#c8f7c8",
            TextSecondaryColor = "#6b8f6b",
            AccentColor = "#22c55e",
            SuccessColor = "#4ade80",
            WarningColor = "#fbbf24",
            DangerColor = "#f87171",
            BorderColor = "#1a3f1a"
        },
        new ThemeColors
        {
            Name = "Ocean",
            DisplayName = "محيط",
            Icon = "🌊",
            BackgroundColor = "#0a1520",
            SurfaceColor = "#0f2030",
            PrimaryColor = "#06b6d4",
            SecondaryColor = "#0ea5e9",
            TextPrimaryColor = "#b0d4f1",
            TextSecondaryColor = "#5090b0",
            AccentColor = "#06b6d4",
            SuccessColor = "#2dd4bf",
            WarningColor = "#fbbf24",
            DangerColor = "#f87171",
            BorderColor = "#1a3040"
        },
        new ThemeColors
        {
            Name = "Sunset",
            DisplayName = "غروب",
            Icon = "🌅",
            BackgroundColor = "#1a0a10",
            SurfaceColor = "#2a1020",
            PrimaryColor = "#f97316",
            SecondaryColor = "#ec4899",
            TextPrimaryColor = "#ffc8d8",
            TextSecondaryColor = "#a07080",
            AccentColor = "#f97316",
            SuccessColor = "#4ade80",
            WarningColor = "#fbbf24",
            DangerColor = "#ef4444",
            BorderColor = "#401020"
        },
        new ThemeColors
        {
            Name = "Rose",
            DisplayName = "وردي",
            Icon = "🌸",
            BackgroundColor = "#1a0a15",
            SurfaceColor = "#2a1025",
            PrimaryColor = "#ec4899",
            SecondaryColor = "#f472b6",
            TextPrimaryColor = "#fce7f3",
            TextSecondaryColor = "#a06080",
            AccentColor = "#ec4899",
            SuccessColor = "#4ade80",
            WarningColor = "#fbbf24",
            DangerColor = "#ef4444",
            BorderColor = "#401030"
        },
        new ThemeColors
        {
            Name = "Golden",
            DisplayName = "ذهبي",
            Icon = "✨",
            BackgroundColor = "#1a150a",
            SurfaceColor = "#2a2010",
            PrimaryColor = "#f59e0b",
            SecondaryColor = "#eab308",
            TextPrimaryColor = "#fef3c7",
            TextSecondaryColor = "#a08050",
            AccentColor = "#f59e0b",
            SuccessColor = "#4ade80",
            WarningColor = "#fb923c",
            DangerColor = "#ef4444",
            BorderColor = "#403010"
        }
    };

    public void ApplyTheme(string themeName)
    {
        var theme = AvailableThemes.FirstOrDefault(t => t.Name == themeName) ?? AvailableThemes[0];
        
        Application.Current!.Resources["BackgroundColor"] = Color.FromArgb(theme.BackgroundColor);
        Application.Current.Resources["SurfaceColor"] = Color.FromArgb(theme.SurfaceColor);
        Application.Current.Resources["PrimaryColor"] = Color.FromArgb(theme.PrimaryColor);
        Application.Current.Resources["SecondaryColor"] = Color.FromArgb(theme.SecondaryColor);
        Application.Current.Resources["TextPrimaryColor"] = Color.FromArgb(theme.TextPrimaryColor);
        Application.Current.Resources["TextSecondaryColor"] = Color.FromArgb(theme.TextSecondaryColor);
        Application.Current.Resources["AccentColor"] = Color.FromArgb(theme.AccentColor);
        Application.Current.Resources["SuccessColor"] = Color.FromArgb(theme.SuccessColor);
        Application.Current.Resources["WarningColor"] = Color.FromArgb(theme.WarningColor);
        Application.Current.Resources["DangerColor"] = Color.FromArgb(theme.DangerColor);
        Application.Current.Resources["BorderColor"] = Color.FromArgb(theme.BorderColor);
    }

    public ThemeColors GetTheme(string themeName)
    {
        return AvailableThemes.FirstOrDefault(t => t.Name == themeName) ?? AvailableThemes[0];
    }
}
