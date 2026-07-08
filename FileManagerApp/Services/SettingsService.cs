using Newtonsoft.Json;
using FileManagerApp.Models;

namespace FileManagerApp.Services;

public class SettingsService
{
    private readonly string _settingsPath;
    private AppSettings _settings;

    public AppSettings Settings => _settings;
    public string CurrentTheme => _settings.Theme;
    public string CurrentLanguage => _settings.Language;
    public int FontSize => _settings.FontSize;
    public string FontFamily => _settings.FontFamily;
    public bool ShowHiddenFiles => _settings.ShowHiddenFiles;
    public string ViewMode => _settings.ViewMode;

    public event EventHandler? SettingsChanged;

    public SettingsService()
    {
        _settingsPath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");
        _settings = LoadSettings();
    }

    private AppSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch { }
        return new AppSettings();
    }

    public void SaveSettings()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(_settingsPath, json);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
        catch { }
    }

    public void SetTheme(string theme)
    {
        _settings.Theme = theme;
        SaveSettings();
    }

    public void SetLanguage(string language)
    {
        _settings.Language = language;
        SaveSettings();
    }

    public void SetFontSize(int size)
    {
        _settings.FontSize = Math.Clamp(size, 10, 28);
        SaveSettings();
    }

    public void SetFontFamily(string family)
    {
        _settings.FontFamily = family;
        SaveSettings();
    }

    public void SetAccentColor(string color)
    {
        _settings.AccentColor = color;
        SaveSettings();
    }

    public void SetShowHiddenFiles(bool show)
    {
        _settings.ShowHiddenFiles = show;
        SaveSettings();
    }

    public void SetViewMode(string mode)
    {
        _settings.ViewMode = mode;
        SaveSettings();
    }

    public void SetSortBy(string sortBy)
    {
        _settings.SortBy = sortBy;
        SaveSettings();
    }

    public void SetEditorFontSize(int size)
    {
        _settings.EditorFontSize = Math.Clamp(size, 10, 32);
        SaveSettings();
    }

    public void SetEditorWordWrap(bool wrap)
    {
        _settings.EditorWordWrap = wrap;
        SaveSettings();
    }

    public void AddFavorite(string path)
    {
        if (!_settings.FavoritePaths.Contains(path))
        {
            _settings.FavoritePaths.Add(path);
            SaveSettings();
        }
    }

    public void RemoveFavorite(string path)
    {
        _settings.FavoritePaths.Remove(path);
        SaveSettings();
    }

    public bool IsFavorite(string path) => _settings.FavoritePaths.Contains(path);

    public void AddRecent(string path)
    {
        _settings.RecentPaths.Remove(path);
        _settings.RecentPaths.Insert(0, path);
        if (_settings.RecentPaths.Count > 20)
            _settings.RecentPaths = _settings.RecentPaths.Take(20).ToList();
        SaveSettings();
    }
}
