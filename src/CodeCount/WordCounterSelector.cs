namespace CodeCount;

using Microsoft.Extensions.FileSystemGlobbing;

public interface IWordCounterSelector
{
    void RegisterWordCounter(IEnumerable<string> filters, IWordCounter wordCounter);
    IWordCounter? SelectWordCounter(string filePath);
}

public class WordCounterSelector : IWordCounterSelector
{
    // This is a list so if a path is registered multiple times, the first one will be used.
    private readonly List<(Matcher, IWordCounter)> _wordCounters = new();

    public void RegisterWordCounter(IEnumerable<string> filters, IWordCounter wordCounter)
    {
        if (wordCounter is null)
        {
            throw new ArgumentNullException(nameof(wordCounter));
        }

        if (filters is null)
        {
            throw new ArgumentNullException(nameof(filters));
        }

        if (!filters.Any())
        {
            throw new ArgumentException($"{nameof(filters)} cannot be empty.", nameof(filters));
        }

        var matcher = new Matcher();
        matcher.AddIncludePatterns(filters);

        _wordCounters.Add((matcher, wordCounter));
    }

    public IWordCounter? SelectWordCounter(string filePath)
    {
        foreach (var (matcher, wordCounter) in _wordCounters)
        {
            if (matcher.Match("/", filePath).HasMatches)
            {
                return wordCounter;
            }
        }

        return null;
    }
}
