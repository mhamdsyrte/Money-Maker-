using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerApp.Services;

namespace FileManagerApp.ViewModels;

[QueryProperty(nameof(FilePath), "path")]
public partial class TextEditorViewModel : BaseViewModel
{
    private readonly FileService _fileService;
    private readonly SettingsService _settingsService;
    private string _originalContent = string.Empty;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _hasChanges;

    [ObservableProperty]
    private int _fontSize;

    [ObservableProperty]
    private bool _wordWrap;

    [ObservableProperty]
    private bool _showLineNumbers;

    [ObservableProperty]
    private string _language = "plaintext";

    [ObservableProperty]
    private int _lineCount;

    [ObservableProperty]
    private int _charCount;

    public TextEditorViewModel(FileService fileService, SettingsService settingsService)
    {
        _fileService = fileService;
        _settingsService = settingsService;
        FontSize = _settingsService.Settings.EditorFontSize;
        WordWrap = _settingsService.Settings.EditorWordWrap;
        ShowLineNumbers = _settingsService.Settings.EditorLineNumbers;
    }

    partial void OnFilePathChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var decoded = Uri.UnescapeDataString(value);
            FilePath = decoded;
            FileName = Path.GetFileName(decoded);
            Title = FileName;
            Language = GetLanguage(Path.GetExtension(decoded));
            LoadContent();
        }
    }

    partial void OnContentChanged(string value)
    {
        HasChanges = value != _originalContent;
        LineCount = value?.Split('\n').Length ?? 0;
        CharCount = value?.Length ?? 0;
    }

    [RelayCommand]
    public async Task LoadContent()
    {
        IsBusy = true;
        try
        {
            Content = await _fileService.ReadFile(FilePath);
            _originalContent = Content;
            HasChanges = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task Save()
    {
        IsBusy = true;
        try
        {
            var success = await _fileService.WriteFile(FilePath, Content);
            if (success)
            {
                _originalContent = Content;
                HasChanges = false;
                await Shell.Current.DisplayAlert("نجاح", "تم حفظ الملف", "حسناً");
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل حفظ الملف", "حسناً");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void IncreaseFontSize()
    {
        FontSize = Math.Min(32, FontSize + 2);
        _settingsService.SetEditorFontSize(FontSize);
    }

    [RelayCommand]
    public void DecreaseFontSize()
    {
        FontSize = Math.Max(10, FontSize - 2);
        _settingsService.SetEditorFontSize(FontSize);
    }

    [RelayCommand]
    public void ToggleWordWrap()
    {
        WordWrap = !WordWrap;
        _settingsService.SetEditorWordWrap(WordWrap);
    }

    [RelayCommand]
    public void ToggleLineNumbers()
    {
        ShowLineNumbers = !ShowLineNumbers;
    }

    [RelayCommand]
    public async Task GoBack()
    {
        if (HasChanges)
        {
            var save = await Shell.Current.DisplayAlert("تغييرات غير محفوظة", "هل تريد حفظ التغييرات؟", "حفظ", "تجاهل");
            if (save) await Save();
        }
        await Shell.Current.GoToAsync("..");
    }

    private string GetLanguage(string extension)
    {
        return extension.ToLower() switch
        {
            ".py" => "Python",
            ".js" => "JavaScript",
            ".ts" => "TypeScript",
            ".jsx" or ".tsx" => "React",
            ".html" or ".htm" => "HTML",
            ".css" => "CSS",
            ".scss" or ".sass" => "SCSS",
            ".cs" => "C#",
            ".java" => "Java",
            ".kt" => "Kotlin",
            ".swift" => "Swift",
            ".go" => "Go",
            ".rs" => "Rust",
            ".rb" => "Ruby",
            ".php" => "PHP",
            ".c" => "C",
            ".cpp" or ".cc" => "C++",
            ".sql" => "SQL",
            ".sh" or ".bash" => "Bash",
            ".json" => "JSON",
            ".xml" => "XML",
            ".yaml" or ".yml" => "YAML",
            ".md" => "Markdown",
            ".dart" => "Dart",
            _ => "Text"
        };
    }
}
