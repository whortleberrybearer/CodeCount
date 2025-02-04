public interface IFileSearcher
{
    IEnumerable<string> GetAllFiles(string directoryPath);
}

public class FileSearcher : IFileSearcher
{
    public IEnumerable<string> GetAllFiles(string directoryPath)
    {
        return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);
    }
}