public interface IFileSearcher
{
    IEnumerable<FileInfo> GetAllFiles(string directoryPath);
}

public class FileSearcher : IFileSearcher
{
    public IEnumerable<FileInfo> GetAllFiles(string directoryPath)
    {
        var directoryInfo = new DirectoryInfo(directoryPath);

        return directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
    }
}