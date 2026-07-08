using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace FileManagerApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Cairo-Regular.ttf", "CairoRegular");
                fonts.AddFont("Cairo-Bold.ttf", "CairoBold");
                fonts.AddFont("Cairo-SemiBold.ttf", "CairoSemiBold");
                fonts.AddFont("JetBrainsMono-Regular.ttf", "JetBrainsMono");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register Services
        builder.Services.AddSingleton<Services.SettingsService>();
        builder.Services.AddSingleton<Services.FileService>();
        builder.Services.AddSingleton<Services.ThemeService>();

        // Register ViewModels
        builder.Services.AddTransient<ViewModels.MainViewModel>();
        builder.Services.AddTransient<ViewModels.FileBrowserViewModel>();
        builder.Services.AddTransient<ViewModels.TextEditorViewModel>();
        builder.Services.AddTransient<ViewModels.ImageEditorViewModel>();
        builder.Services.AddTransient<ViewModels.VideoEditorViewModel>();
        builder.Services.AddTransient<ViewModels.SettingsViewModel>();

        // Register Pages
        builder.Services.AddTransient<Views.MainPage>();
        builder.Services.AddTransient<Views.FileBrowserPage>();
        builder.Services.AddTransient<Views.TextEditorPage>();
        builder.Services.AddTransient<Views.ImageEditorPage>();
        builder.Services.AddTransient<Views.VideoEditorPage>();
        builder.Services.AddTransient<Views.SettingsPage>();

        return builder.Build();
    }
}
