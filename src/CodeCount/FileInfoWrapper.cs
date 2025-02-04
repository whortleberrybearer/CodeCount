// This wrapper class is required to allow mocking of the FileInfo class in unit tests.
public class FileInfoWrapper : IFileInfo
{
    private readonly FileInfo _fileInfo;

    public FileInfoWrapper(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    public string FullName => _fileInfo.FullName;

    public bool HasValidExtension(IEnumerable<string> validExtensions)
    {
        if (validExtensions is null)
        {
            throw new ArgumentNullException(nameof(validExtensions));
        }

        return validExtensions.Contains(_fileInfo.Extension, StringComparer.OrdinalIgnoreCase);
    }

    public IDictionary<string, int> GetWordCounts(IWordCounter wordCounter)
    {
        using (var stream = _fileInfo.OpenRead())
        {
            return wordCounter.GetWordCounts(stream);
        }
    }
}
