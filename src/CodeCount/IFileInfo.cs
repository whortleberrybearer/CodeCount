public interface IFileInfo
{
    string FullName { get; }
    Stream OpenRead();
    IDictionary<string, int> GetWordCounts(IWordCounter wordCounter);
}
