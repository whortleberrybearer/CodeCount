public interface IFileSearcher
{
    IEnumerable<IFileInfo> GetAllFiles(string directoryPath);
}

public class FileSearcher : IFileSearcher
{
    public IEnumerable<IFileInfo> GetAllFiles(string directoryPath)
    {
        var directoryInfo = new DirectoryInfo(directoryPath);

        return directoryInfo
            .GetFiles("*.*", SearchOption.AllDirectories)
            .Select(fileInfo => new FileInfoWrapper(fileInfo));
    }
}