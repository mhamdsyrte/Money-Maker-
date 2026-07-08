using FileManagerApp.Models;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO.Compression;

namespace FileManagerApp.Services;

public class FileService
{
    private readonly SettingsService _settings;

    public FileService(SettingsService settings)
    {
        _settings = settings;
    }

    public string GetDefaultPath()
    {
#if ANDROID
        return Android.OS.Environment.ExternalStorageDirectory?.AbsolutePath ?? "/storage/emulated/0";
#elif IOS || MACCATALYST
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif WINDOWS
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
#else
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
    }

    public List<FileItem> GetFiles(string path)
    {
        var items = new List<FileItem>();

        try
        {
            var dirInfo = new DirectoryInfo(path);

            // Get directories
            foreach (var dir in dirInfo.GetDirectories())
            {
                if (!_settings.ShowHiddenFiles && (dir.Attributes & FileAttributes.Hidden) != 0)
                    continue;

                items.Add(new FileItem
                {
                    Name = dir.Name,
                    FullPath = dir.FullName,
                    IsDirectory = true,
                    LastModified = dir.LastWriteTime,
                    IsHidden = (dir.Attributes & FileAttributes.Hidden) != 0,
                    IsFavorite = _settings.IsFavorite(dir.FullName)
                });
            }

            // Get files
            foreach (var file in dirInfo.GetFiles())
            {
                if (!_settings.ShowHiddenFiles && (file.Attributes & FileAttributes.Hidden) != 0)
                    continue;

                items.Add(new FileItem
                {
                    Name = file.Name,
                    FullPath = file.FullName,
                    IsDirectory = false,
                    Size = file.Length,
                    LastModified = file.LastWriteTime,
                    Extension = file.Extension,
                    IsHidden = (file.Attributes & FileAttributes.Hidden) != 0,
                    IsFavorite = _settings.IsFavorite(file.FullName)
                });
            }

            // Sort
            items = _settings.Settings.SortBy switch
            {
                "Name" => items.OrderBy(f => !f.IsDirectory).ThenBy(f => f.Name).ToList(),
                "Size" => items.OrderBy(f => !f.IsDirectory).ThenBy(f => f.Size).ToList(),
                "Date" => items.OrderBy(f => !f.IsDirectory).ThenByDescending(f => f.LastModified).ToList(),
                "Type" => items.OrderBy(f => !f.IsDirectory).ThenBy(f => f.Extension).ToList(),
                _ => items
            };

            if (_settings.Settings.SortDescending)
                items.Reverse();
        }
        catch (UnauthorizedAccessException)
        {
            // No access to this directory
        }
        catch (Exception)
        {
            // Other errors
        }

        return items;
    }

    public async Task<bool> CreateFolder(string path, string name)
    {
        try
        {
            var fullPath = Path.Combine(path, name);
            Directory.CreateDirectory(fullPath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CreateFile(string path, string name, string content = "")
    {
        try
        {
            var fullPath = Path.Combine(path, name);
            await File.WriteAllTextAsync(fullPath, content);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(string path)
    {
        try
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            else if (File.Exists(path))
                File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Rename(string path, string newName)
    {
        try
        {
            var directory = Path.GetDirectoryName(path);
            var newPath = Path.Combine(directory!, newName);

            if (Directory.Exists(path))
                Directory.Move(path, newPath);
            else if (File.Exists(path))
                File.Move(path, newPath);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Copy(string source, string destination)
    {
        try
        {
            if (File.Exists(source))
            {
                var destFile = Path.Combine(destination, Path.GetFileName(source));
                File.Copy(source, destFile, true);
            }
            else if (Directory.Exists(source))
            {
                var destDir = Path.Combine(destination, Path.GetFileName(source));
                CopyDirectory(source, destDir);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Move(string source, string destination)
    {
        try
        {
            if (File.Exists(source))
            {
                var destFile = Path.Combine(destination, Path.GetFileName(source));
                File.Move(source, destFile);
            }
            else if (Directory.Exists(source))
            {
                var destDir = Path.Combine(destination, Path.GetFileName(source));
                Directory.Move(source, destDir);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);

        foreach (var file in Directory.GetFiles(source))
        {
            var destFile = Path.Combine(destination, Path.GetFileName(file));
            File.Copy(file, destFile);
        }

        foreach (var dir in Directory.GetDirectories(source))
        {
            var destDir = Path.Combine(destination, Path.GetFileName(dir));
            CopyDirectory(dir, destDir);
        }
    }

    public async Task<string> ReadFile(string path)
    {
        try
        {
            return await File.ReadAllTextAsync(path);
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> WriteFile(string path, string content)
    {
        try
        {
            await File.WriteAllTextAsync(path, content);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<FileItem> Search(string path, string query)
    {
        var results = new List<FileItem>();

        try
        {
            var files = Directory.GetFiles(path, $"*{query}*", SearchOption.AllDirectories);
            var dirs = Directory.GetDirectories(path, $"*{query}*", SearchOption.AllDirectories);

            foreach (var dir in dirs)
            {
                var info = new DirectoryInfo(dir);
                results.Add(new FileItem
                {
                    Name = info.Name,
                    FullPath = info.FullName,
                    IsDirectory = true,
                    LastModified = info.LastWriteTime
                });
            }

            foreach (var file in files)
            {
                var info = new FileInfo(file);
                results.Add(new FileItem
                {
                    Name = info.Name,
                    FullPath = info.FullName,
                    IsDirectory = false,
                    Size = info.Length,
                    LastModified = info.LastWriteTime,
                    Extension = info.Extension
                });
            }
        }
        catch { }

        return results;
    }

    /// <summary>
    /// يستخرج أرشيف (zip, rar, 7z, tar, gz) إلى مجلد جديد بجانبه بنفس اسم الملف.
    /// يرجع مسار مجلد الاستخراج عند النجاح أو null عند الفشل.
    /// </summary>
    public async Task<string?> ExtractArchive(string archivePath, IProgress<int>? progress = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                var directory = Path.GetDirectoryName(archivePath)!;
                var name = Path.GetFileNameWithoutExtension(archivePath);
                // .tar.gz => اسم بدون .tar كمان
                if (name.EndsWith(".tar", StringComparison.OrdinalIgnoreCase))
                    name = Path.GetFileNameWithoutExtension(name);

                var destination = Path.Combine(directory, name);
                var uniqueDestination = destination;
                var counter = 1;
                while (Directory.Exists(uniqueDestination))
                {
                    uniqueDestination = $"{destination} ({counter})";
                    counter++;
                }
                Directory.CreateDirectory(uniqueDestination);

                using var archive = ArchiveFactory.Open(archivePath);
                var entries = archive.Entries.Where(e => !e.IsDirectory).ToList();
                var total = Math.Max(entries.Count, 1);
                var done = 0;

                foreach (var entry in entries)
                {
                    entry.WriteToDirectory(uniqueDestination, new SharpCompress.Common.ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                    done++;
                    progress?.Report((int)((done / (double)total) * 100));
                }

                return uniqueDestination;
            }
            catch
            {
                return null;
            }
        });
    }

    /// <summary>
    /// يضغط ملف أو مجلد إلى أرشيف ZIP بجانبه.
    /// </summary>
    public async Task<bool> CompressToZip(string sourcePath, string? destinationZipPath = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                var directory = Path.GetDirectoryName(sourcePath)!;
                var name = Path.GetFileName(sourcePath);
                var zipPath = destinationZipPath ?? Path.Combine(directory, $"{name}.zip");

                var uniqueZipPath = zipPath;
                var counter = 1;
                while (File.Exists(uniqueZipPath))
                {
                    uniqueZipPath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(zipPath)} ({counter}).zip");
                    counter++;
                }

                if (File.Exists(sourcePath))
                {
                    using var zip = ZipFile.Open(uniqueZipPath, ZipArchiveMode.Create);
                    zip.CreateEntryFromFile(sourcePath, Path.GetFileName(sourcePath));
                }
                else if (Directory.Exists(sourcePath))
                {
                    ZipFile.CreateFromDirectory(sourcePath, uniqueZipPath);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        });
    }

    public bool IsArchiveFile(string path)
    {
        var ext = Path.GetExtension(path).ToLower();
        return ext is ".zip" or ".rar" or ".7z" or ".tar" or ".gz" or ".tgz" or ".bz2";
    }

    public string GetFileType(string path)
    {
        var ext = Path.GetExtension(path).ToLower();
        return ext switch
        {
            ".txt" or ".md" or ".json" or ".xml" or ".yaml" or ".yml" or ".log" or
            ".py" or ".js" or ".ts" or ".cs" or ".java" or ".cpp" or ".c" or ".h" or
            ".html" or ".css" or ".scss" or ".sql" or ".sh" or ".bash" or ".php" or
            ".rb" or ".go" or ".rs" or ".kt" or ".swift" or ".dart" => "text",
            
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => "image",
            
            ".mp4" or ".avi" or ".mkv" or ".mov" or ".wmv" or ".flv" or ".webm" => "video",
            
            ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" => "audio",
            
            _ => "unknown"
        };
    }
}
