namespace FileManagerApp.Models;

public class FileItem
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Extension { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public bool IsHidden { get; set; }

    public string Icon => GetIcon();
    public string SizeFormatted => FormatSize(Size);
    public string DateFormatted => LastModified.ToString("yyyy/MM/dd HH:mm");

    public bool IsArchive => !IsDirectory && Extension.ToLower() is ".zip" or ".rar" or ".7z" or ".tar" or ".gz" or ".tgz" or ".bz2";

    private string GetIcon()
    {
        if (IsDirectory) return "📁";

        return Extension.ToLower() switch
        {
            // Programming Languages
            ".py" => "🐍",
            ".js" => "📜",
            ".ts" => "💠",
            ".jsx" or ".tsx" => "⚛️",
            ".html" or ".htm" => "🌐",
            ".css" or ".scss" or ".sass" => "🎨",
            ".cs" => "🟢",
            ".java" => "☕",
            ".kt" => "🟣",
            ".swift" => "🦅",
            ".go" => "🐹",
            ".rs" => "🦀",
            ".rb" => "💎",
            ".php" => "🐘",
            ".c" or ".cpp" or ".h" => "⚙️",
            ".r" => "📊",
            ".lua" => "🌙",
            ".dart" => "🎯",
            ".sql" => "🗄️",
            ".sh" or ".bash" => "🖥️",
            ".ps1" => "💻",
            ".json" => "📋",
            ".xml" => "📋",
            ".yaml" or ".yml" => "📋",
            ".md" => "📝",
            ".txt" or ".log" => "📄",

            // Images
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => "🖼️",
            ".ico" => "🎨",
            ".psd" => "🎭",
            ".ai" => "✨",

            // Videos
            ".mp4" or ".avi" or ".mkv" or ".mov" or ".wmv" or ".flv" or ".webm" => "🎬",

            // Audio
            ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" => "🎵",

            // Archives
            ".zip" or ".rar" or ".7z" or ".tar" or ".gz" => "📦",

            // Documents
            ".pdf" => "📕",
            ".doc" or ".docx" => "📄",
            ".xls" or ".xlsx" => "📊",
            ".ppt" or ".pptx" => "📽️",

            // Executables
            ".exe" or ".msi" => "⚡",
            ".apk" => "🤖",
            ".dmg" => "🍎",
            ".deb" or ".rpm" => "🐧",

            // Fonts
            ".ttf" or ".otf" or ".woff" => "🔤",

            _ => "📄"
        };
    }

    private static string FormatSize(long bytes)
    {
        if (bytes == 0) return "0 B";
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(1024));
        return $"{Math.Round(bytes / Math.Pow(1024, i), 2)} {sizes[i]}";
    }
}
