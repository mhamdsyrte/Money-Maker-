namespace FileManagerApp;

public partial class App : Application
{
    public static Services.SettingsService Settings { get; private set; } = null!;
    public static Services.ThemeService ThemeService { get; private set; } = null!;

    public App(Services.SettingsService settings, Services.ThemeService themeService)
    {
        InitializeComponent();
        Settings = settings;
        ThemeService = themeService;
        
        // Apply saved theme
        ThemeService.ApplyTheme(Settings.CurrentTheme);
        
        MainPage = new AppShell();
    }
}
