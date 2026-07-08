using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerApp.Models;
using FileManagerApp.Services;
using System.Collections.ObjectModel;

namespace FileManagerApp.ViewModels;

public partial class FileBrowserViewModel : BaseViewModel
{
    private readonly FileService _fileService;
    private readonly SettingsService _settingsService;
    private readonly Stack<string> _navigationStack = new();

    [ObservableProperty]
    private string _currentPath = string.Empty;

    [ObservableProperty]
    private ObservableCollection<FileItem> _files = new();

    [ObservableProperty]
    private FileItem? _selectedFile;

    [ObservableProperty]
    private bool _isGridView = true;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private FileItem? _clipboardItem;

    [ObservableProperty]
    private bool _isCutOperation;

    public bool CanGoBack => _navigationStack.Count > 0;

    public FileBrowserViewModel(FileService fileService, SettingsService settingsService)
    {
        _fileService = fileService;
        _settingsService = settingsService;
        Title = "الملفات";
        IsGridView = _settingsService.ViewMode == "Grid";
        CurrentPath = _fileService.GetDefaultPath();
    }

    [RelayCommand]
    public void LoadFiles()
    {
        IsBusy = true;
        try
        {
            var items = _fileService.GetFiles(CurrentPath);
            Files = new ObservableCollection<FileItem>(items);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task OpenItem(FileItem item)
    {
        if (item.IsDirectory)
        {
            _navigationStack.Push(CurrentPath);
            CurrentPath = item.FullPath;
            LoadFiles();
            OnPropertyChanged(nameof(CanGoBack));
        }
        else
        {
            var fileType = _fileService.GetFileType(item.FullPath);
            _settingsService.AddRecent(item.FullPath);

            switch (fileType)
            {
                case "text":
                    await Shell.Current.GoToAsync($"{nameof(Views.TextEditorPage)}?path={Uri.EscapeDataString(item.FullPath)}");
                    break;
                case "image":
                    await Shell.Current.GoToAsync($"{nameof(Views.ImageEditorPage)}?path={Uri.EscapeDataString(item.FullPath)}");
                    break;
                case "video":
                case "audio":
                    await Shell.Current.GoToAsync($"{nameof(Views.VideoEditorPage)}?path={Uri.EscapeDataString(item.FullPath)}");
                    break;
                default:
                    await Shell.Current.DisplayAlert("غير مدعوم", "نوع الملف غير مدعوم للعرض", "حسناً");
                    break;
            }
        }
    }

    [RelayCommand]
    public void GoBack()
    {
        if (_navigationStack.Count > 0)
        {
            CurrentPath = _navigationStack.Pop();
            LoadFiles();
            OnPropertyChanged(nameof(CanGoBack));
        }
    }

    [RelayCommand]
    public void GoUp()
    {
        var parent = Directory.GetParent(CurrentPath);
        if (parent != null)
        {
            _navigationStack.Push(CurrentPath);
            CurrentPath = parent.FullName;
            LoadFiles();
            OnPropertyChanged(nameof(CanGoBack));
        }
    }

    [RelayCommand]
    public void GoHome()
    {
        _navigationStack.Clear();
        CurrentPath = _fileService.GetDefaultPath();
        LoadFiles();
        OnPropertyChanged(nameof(CanGoBack));
    }

    [RelayCommand]
    public void ToggleViewMode()
    {
        IsGridView = !IsGridView;
        _settingsService.SetViewMode(IsGridView ? "Grid" : "List");
    }

    [RelayCommand]
    public async Task CreateFolder()
    {
        var name = await Shell.Current.DisplayPromptAsync("مجلد جديد", "أدخل اسم المجلد:", "إنشاء", "إلغاء");
        if (!string.IsNullOrWhiteSpace(name))
        {
            var success = await _fileService.CreateFolder(CurrentPath, name);
            if (success)
            {
                LoadFiles();
                await Shell.Current.DisplayAlert("نجاح", "تم إنشاء المجلد", "حسناً");
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل إنشاء المجلد", "حسناً");
            }
        }
    }

    [RelayCommand]
    public async Task CreateFile()
    {
        var name = await Shell.Current.DisplayPromptAsync("ملف جديد", "أدخل اسم الملف:", "إنشاء", "إلغاء", placeholder: "ملف.txt");
        if (!string.IsNullOrWhiteSpace(name))
        {
            var success = await _fileService.CreateFile(CurrentPath, name);
            if (success)
            {
                LoadFiles();
                await Shell.Current.DisplayAlert("نجاح", "تم إنشاء الملف", "حسناً");
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل إنشاء الملف", "حسناً");
            }
        }
    }

    [RelayCommand]
    public async Task DeleteItem(FileItem item)
    {
        var confirm = await Shell.Current.DisplayAlert("تأكيد الحذف", $"هل تريد حذف \"{item.Name}\"؟", "حذف", "إلغاء");
        if (confirm)
        {
            var success = await _fileService.Delete(item.FullPath);
            if (success)
            {
                Files.Remove(item);
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل حذف العنصر", "حسناً");
            }
        }
    }

    [RelayCommand]
    public async Task RenameItem(FileItem item)
    {
        var newName = await Shell.Current.DisplayPromptAsync("تغيير الاسم", "الاسم الجديد:", "حفظ", "إلغاء", initialValue: item.Name);
        if (!string.IsNullOrWhiteSpace(newName) && newName != item.Name)
        {
            var success = await _fileService.Rename(item.FullPath, newName);
            if (success)
            {
                LoadFiles();
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل تغيير الاسم", "حسناً");
            }
        }
    }

    [RelayCommand]
    public void CopyItem(FileItem item)
    {
        ClipboardItem = item;
        IsCutOperation = false;
    }

    [RelayCommand]
    public void CutItem(FileItem item)
    {
        ClipboardItem = item;
        IsCutOperation = true;
    }

    [RelayCommand]
    public async Task Paste()
    {
        if (ClipboardItem == null) return;

        bool success;
        if (IsCutOperation)
        {
            success = await _fileService.Move(ClipboardItem.FullPath, CurrentPath);
        }
        else
        {
            success = await _fileService.Copy(ClipboardItem.FullPath, CurrentPath);
        }

        if (success)
        {
            if (IsCutOperation) ClipboardItem = null;
            LoadFiles();
        }
        else
        {
            await Shell.Current.DisplayAlert("خطأ", "فشلت العملية", "حسناً");
        }
    }

    [RelayCommand]
    public void ToggleFavorite(FileItem item)
    {
        if (_settingsService.IsFavorite(item.FullPath))
        {
            _settingsService.RemoveFavorite(item.FullPath);
            item.IsFavorite = false;
        }
        else
        {
            _settingsService.AddFavorite(item.FullPath);
            item.IsFavorite = true;
        }
    }

    [RelayCommand]
    public void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            LoadFiles();
            return;
        }

        IsSearching = true;
        IsBusy = true;
        try
        {
            var results = _fileService.Search(CurrentPath, SearchQuery);
            Files = new ObservableCollection<FileItem>(results);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void ClearSearch()
    {
        SearchQuery = string.Empty;
        IsSearching = false;
        LoadFiles();
    }

    [RelayCommand]
    public async Task ExtractItem(FileItem item)
    {
        if (!item.IsArchive)
        {
            await Shell.Current.DisplayAlert("غير مدعوم", "هذا الملف ليس أرشيفًا قابلاً للاستخراج", "حسناً");
            return;
        }

        IsBusy = true;
        try
        {
            var resultPath = await _fileService.ExtractArchive(item.FullPath);
            if (resultPath != null)
            {
                LoadFiles();
                await Shell.Current.DisplayAlert("نجاح", $"تم الاستخراج إلى:\n{Path.GetFileName(resultPath)}", "حسناً");
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل استخراج الأرشيف", "حسناً");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CompressItem(FileItem item)
    {
        IsBusy = true;
        try
        {
            var success = await _fileService.CompressToZip(item.FullPath);
            if (success)
            {
                LoadFiles();
                await Shell.Current.DisplayAlert("نجاح", "تم إنشاء الأرشيف بنجاح", "حسناً");
            }
            else
            {
                await Shell.Current.DisplayAlert("خطأ", "فشل ضغط الملف", "حسناً");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ShowItemInfo(FileItem item)
    {
        var info = $"الاسم: {item.Name}\n" +
                   $"المسار: {item.FullPath}\n" +
                   $"النوع: {(item.IsDirectory ? "مجلد" : item.Extension)}\n" +
                   $"الحجم: {item.SizeFormatted}\n" +
                   $"آخر تعديل: {item.DateFormatted}";

        await Shell.Current.DisplayAlert("معلومات", info, "حسناً");
    }
}
