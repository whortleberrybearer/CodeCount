public interface IFileSearcher
{
    IEnumerable<IFileInfo> GetAllFiles(string directoryPath);
}

public class FileSearcher : IFileSearcher
{
    public string? Filter { get; set; }

    public IEnumerable<IFileInfo> GetAllFiles(string directoryPath)
    {
        var directoryInfo = new DirectoryInfo(directoryPath);

        return directoryInfo
            .GetFiles(Filter ?? "*.*", SearchOption.AllDirectories)
            .Select(fileInfo => new FileInfoWrapper(fileInfo));
    }
}