using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerApp.Models;
using FileManagerApp.Services;
using System.Collections.ObjectModel;

namespace FileManagerApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly SettingsService _settingsService;
    private readonly ThemeService _themeService;

    [ObservableProperty]
    private ObservableCollection<ThemeColors> _themes = new();

    [ObservableProperty]
    private ThemeColors? _selectedTheme;

    [ObservableProperty]
    private int _fontSize;

    [ObservableProperty]
    private string _selectedLanguage = "ar";

    [ObservableProperty]
    private bool _showHiddenFiles;

    [ObservableProperty]
    private string _viewMode = "Grid";

    [ObservableProperty]
    private int _editorFontSize;

    [ObservableProperty]
    private bool _editorWordWrap;

    [ObservableProperty]
    private bool _editorLineNumbers;

    [ObservableProperty]
    private string _selectedAccentColor = "#3b82f6";

    public List<string> Languages { get; } = new() { "ar", "en" };
    
    public List<string> AccentColors { get; } = new()
    {
        "#3b82f6", "#8b5cf6", "#22c55e", "#ef4444",
        "#f59e0b", "#ec4899", "#06b6d4", "#f97316"
    };

    public List<int> FontSizes { get; } = new() { 12, 14, 16, 18, 20, 22, 24 };

    public SettingsViewModel(SettingsService settingsService, ThemeService themeService)
    {
        _settingsService = settingsService;
        _themeService = themeService;
        Title = "الإعدادات";
        LoadSettings();
    }

    private void LoadSettings()
    {
        Themes = new ObservableCollection<ThemeColors>(_themeService.AvailableThemes);
        SelectedTheme = Themes.FirstOrDefault(t => t.Name == _settingsService.CurrentTheme) ?? Themes[0];
        FontSize = _settingsService.FontSize;
        SelectedLanguage = _settingsService.CurrentLanguage;
        ShowHiddenFiles = _settingsService.ShowHiddenFiles;
        ViewMode = _settingsService.ViewMode;
        EditorFontSize = _settingsService.Settings.EditorFontSize;
        EditorWordWrap = _settingsService.Settings.EditorWordWrap;
        EditorLineNumbers = _settingsService.Settings.EditorLineNumbers;
        SelectedAccentColor = _settingsService.Settings.AccentColor;
    }

    partial void OnSelectedThemeChanged(ThemeColors? value)
    {
        if (value != null)
        {
            _settingsService.SetTheme(value.Name);
            _themeService.ApplyTheme(value.Name);
        }
    }

    partial void OnFontSizeChanged(int value)
    {
        _settingsService.SetFontSize(value);
    }

    partial void OnSelectedLanguageChanged(string value)
    {
        _settingsService.SetLanguage(value);
    }

    partial void OnShowHiddenFilesChanged(bool value)
    {
        _settingsService.SetShowHiddenFiles(value);
    }

    partial void OnViewModeChanged(string value)
    {
        _settingsService.SetViewMode(value);
    }

    partial void OnEditorFontSizeChanged(int value)
    {
        _settingsService.SetEditorFontSize(value);
    }

    partial void OnEditorWordWrapChanged(bool value)
    {
        _settingsService.SetEditorWordWrap(value);
    }

    partial void OnSelectedAccentColorChanged(string value)
    {
        _settingsService.SetAccentColor(value);
    }

    [RelayCommand]
    public void SelectTheme(ThemeColors theme)
    {
        SelectedTheme = theme;
    }

    [RelayCommand]
    public void SelectAccentColor(string color)
    {
        SelectedAccentColor = color;
    }

    [RelayCommand]
    public void IncreaseFontSize()
    {
        FontSize = Math.Min(28, FontSize + 2);
    }

    [RelayCommand]
    public void DecreaseFontSize()
    {
        FontSize = Math.Max(10, FontSize - 2);
    }

    [RelayCommand]
    public void ToggleViewMode()
    {
        ViewMode = ViewMode == "Grid" ? "List" : "Grid";
    }

    [RelayCommand]
    public async Task ResetSettings()
    {
        var confirm = await Shell.Current.DisplayAlert("تأكيد", "هل تريد إعادة تعيين جميع الإعدادات؟", "نعم", "لا");
        if (confirm)
        {
            _settingsService.SetTheme("Dark");
            _settingsService.SetFontSize(14);
            _settingsService.SetLanguage("ar");
            _settingsService.SetShowHiddenFiles(false);
            _settingsService.SetViewMode("Grid");
            _settingsService.SetEditorFontSize(14);
            _settingsService.SetEditorWordWrap(true);
            _settingsService.SetAccentColor("#3b82f6");
            
            LoadSettings();
            _themeService.ApplyTheme("Dark");
            
            await Shell.Current.DisplayAlert("نجاح", "تم إعادة تعيين الإعدادات", "حسناً");
        }
    }

    [RelayCommand]
    public async Task ShowAbout()
    {
        await Shell.Current.DisplayAlert(
            "حول التطبيق",
            "مدير الملفات Z-Pro\n\n" +
            "الإصدار: 1.0.0\n\n" +
            "مدير ملفات مجاني ومتكامل يشمل:\n" +
            "• تصفح وإدارة الملفات\n" +
            "• محرر نصوص مع تلوين الأكواد\n" +
            "• عارض ومحرر الصور\n" +
            "• مشغل ومحرر الفيديو\n" +
            "• ثيمات متعددة\n" +
            "• دعم كامل للعربية\n\n" +
            "مجاني للجميع 💚",
            "حسناً"
        );
    }
}
