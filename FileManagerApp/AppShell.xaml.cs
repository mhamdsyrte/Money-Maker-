namespace FileManagerApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(Views.TextEditorPage), typeof(Views.TextEditorPage));
        Routing.RegisterRoute(nameof(Views.ImageEditorPage), typeof(Views.ImageEditorPage));
        Routing.RegisterRoute(nameof(Views.VideoEditorPage), typeof(Views.VideoEditorPage));
        Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));
    }
}
