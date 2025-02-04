// This wrapper class is required to allow mocking of the FileInfo class in unit tests.
public class FileInfoWrapper : IFileInfo
{
    private readonly FileInfo _fileInfo;

    public FileInfoWrapper(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    public string FullName => _fileInfo.FullName;

    public Stream OpenRead() => _fileInfo.OpenRead();
}
