using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FileManagerApp.ViewModels;

[QueryProperty(nameof(FilePath), "path")]
public partial class VideoEditorViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private bool _isPlaying;

    [ObservableProperty]
    private TimeSpan _currentPosition;

    [ObservableProperty]
    private TimeSpan _duration;

    [ObservableProperty]
    private double _volume = 0.8;

    [ObservableProperty]
    private double _playbackSpeed = 1.0;

    [ObservableProperty]
    private bool _showControls = true;

    [ObservableProperty]
    private double _trimStart;

    [ObservableProperty]
    private double _trimEnd = 100;

    [ObservableProperty]
    private double _brightness = 1.0;

    [ObservableProperty]
    private double _contrast = 1.0;

    [ObservableProperty]
    private string _mediaInfo = string.Empty;

    public string CurrentPositionFormatted => FormatTime(CurrentPosition);
    public string DurationFormatted => FormatTime(Duration);

    public VideoEditorViewModel()
    {
        Title = "مشغل الفيديو";
    }

    partial void OnFilePathChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var decoded = Uri.UnescapeDataString(value);
            FilePath = decoded;
            FileName = Path.GetFileName(decoded);
            Title = FileName;
            LoadMediaInfo();
        }
    }

    partial void OnCurrentPositionChanged(TimeSpan value)
    {
        OnPropertyChanged(nameof(CurrentPositionFormatted));
    }

    partial void OnDurationChanged(TimeSpan value)
    {
        OnPropertyChanged(nameof(DurationFormatted));
    }

    [RelayCommand]
    public void LoadMediaInfo()
    {
        try
        {
            var info = new FileInfo(FilePath);
            MediaInfo = $"الحجم: {FormatSize(info.Length)}";
        }
        catch { }
    }

    [RelayCommand]
    public void TogglePlayPause()
    {
        IsPlaying = !IsPlaying;
    }

    [RelayCommand]
    public void Stop()
    {
        IsPlaying = false;
        CurrentPosition = TimeSpan.Zero;
    }

    [RelayCommand]
    public void SkipForward()
    {
        CurrentPosition += TimeSpan.FromSeconds(10);
    }

    [RelayCommand]
    public void SkipBackward()
    {
        CurrentPosition -= TimeSpan.FromSeconds(10);
    }

    [RelayCommand]
    public void SetSpeed(double speed)
    {
        PlaybackSpeed = speed;
    }

    [RelayCommand]
    public void ToggleControls()
    {
        ShowControls = !ShowControls;
    }

    [RelayCommand]
    public void ResetFilters()
    {
        Brightness = 1.0;
        Contrast = 1.0;
        TrimStart = 0;
        TrimEnd = 100;
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    private static string FormatTime(TimeSpan time)
    {
        return time.Hours > 0
            ? $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}"
            : $"{time.Minutes:D2}:{time.Seconds:D2}";
    }

    private static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        int i = 0;
        double size = bytes;
        while (size >= 1024 && i < sizes.Length - 1)
        {
            size /= 1024;
            i++;
        }
        return $"{size:F2} {sizes[i]}";
    }
}
