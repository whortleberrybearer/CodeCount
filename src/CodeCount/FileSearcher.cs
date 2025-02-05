using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.FileSystemGlobbing;

public interface IFileSearcher
{
    IEnumerable<IFileInfo> GetAllFiles(string directoryPath);
}

public class FileSearcher : IFileSearcher
{
    public string? ExcludeFilter { get; set; }

    public IEnumerable<IFileInfo> GetAllFiles(string directoryPath)
    {
        var matcher = new Matcher();
        matcher.AddInclude("**/*");

        if (!string.IsNullOrEmpty(ExcludeFilter))
        {
            matcher.AddExcludePatterns(ExcludeFilter.Split(';'));
        }

        var matchingFiles = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(directoryPath))).Files;

        return matchingFiles
            .Select(fileMatch => new FileInfoWrapper(new FileInfo(Path.Combine(directoryPath, fileMatch.Path))));
    }
}