public interface IFileInfo
{
    string FullName { get; }
    Stream OpenRead();
}
