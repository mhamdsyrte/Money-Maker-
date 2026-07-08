using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerApp.Services;

namespace FileManagerApp.ViewModels;

[QueryProperty(nameof(FilePath), "path")]
public partial class ImageEditorViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private ImageSource? _imageSource;

    [ObservableProperty]
    private double _brightness = 1.0;

    [ObservableProperty]
    private double _contrast = 1.0;

    [ObservableProperty]
    private double _saturation = 1.0;

    [ObservableProperty]
    private double _rotation = 0;

    [ObservableProperty]
    private double _scale = 1.0;

    [ObservableProperty]
    private bool _showEditor;

    [ObservableProperty]
    private string _imageInfo = string.Empty;

    public ImageEditorViewModel()
    {
        Title = "عارض الصور";
    }

    partial void OnFilePathChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var decoded = Uri.UnescapeDataString(value);
            FilePath = decoded;
            FileName = Path.GetFileName(decoded);
            Title = FileName;
            LoadImage();
        }
    }

    [RelayCommand]
    public void LoadImage()
    {
        try
        {
            ImageSource = ImageSource.FromFile(FilePath);
            var info = new FileInfo(FilePath);
            ImageInfo = $"الحجم: {FormatSize(info.Length)} | التاريخ: {info.LastWriteTime:yyyy/MM/dd}";
        }
        catch (Exception ex)
        {
            ImageInfo = $"خطأ: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ZoomIn()
    {
        Scale = Math.Min(3.0, Scale + 0.1);
    }

    [RelayCommand]
    public void ZoomOut()
    {
        Scale = Math.Max(0.1, Scale - 0.1);
    }

    [RelayCommand]
    public void ResetZoom()
    {
        Scale = 1.0;
    }

    [RelayCommand]
    public void RotateLeft()
    {
        Rotation -= 90;
    }

    [RelayCommand]
    public void RotateRight()
    {
        Rotation += 90;
    }

    [RelayCommand]
    public void ToggleEditor()
    {
        ShowEditor = !ShowEditor;
    }

    [RelayCommand]
    public void ResetFilters()
    {
        Brightness = 1.0;
        Contrast = 1.0;
        Saturation = 1.0;
        Rotation = 0;
        Scale = 1.0;
    }

    [RelayCommand]
    public void ApplyPreset(string preset)
    {
        switch (preset)
        {
            case "warm":
                Brightness = 1.1;
                Contrast = 1.1;
                Saturation = 1.3;
                break;
            case "cool":
                Brightness = 1.0;
                Contrast = 1.0;
                Saturation = 0.9;
                break;
            case "grayscale":
                Saturation = 0;
                break;
            case "vintage":
                Brightness = 0.9;
                Contrast = 1.2;
                Saturation = 0.6;
                break;
            case "vivid":
                Brightness = 1.05;
                Contrast = 1.3;
                Saturation = 1.5;
                break;
        }
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
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
