public class FileSearcher
{
    public IEnumerable<string> GetAllFiles(string directoryPath)
    {
        return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);
    }
}