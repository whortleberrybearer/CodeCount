public interface IFileInfo
{
    string FullName { get; }
    IDictionary<string, int> GetWordCounts(IWordCounter wordCounter);
}
