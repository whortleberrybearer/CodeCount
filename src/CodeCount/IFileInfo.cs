public interface IFileInfo
{
    string FullName { get; }
    bool HasValidExtension(IEnumerable<string> validExtensions);
    IDictionary<string, int> GetWordCounts(IWordCounter wordCounter);
}
