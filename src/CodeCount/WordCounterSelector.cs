public interface IWordCounterSelector
{
    void RegisterWordCounter(string extension, IWordCounter wordCounter);
    IWordCounter? SelectWordCounter(string filePath);
}

public class WordCounterSelector : IWordCounterSelector
{
    private readonly Dictionary<string, IWordCounter> _wordCounters = new();

// TODO: Need to turn this into globs.
    public void RegisterWordCounter(string extension, IWordCounter wordCounter)
    {
        if (wordCounter is null)
        {
            throw new ArgumentNullException(nameof(wordCounter));
        }

        if (extension is null)
        {
            throw new ArgumentNullException(nameof(extension));
        }

        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentException("Extension cannot be empty.", nameof(extension));
        }

        _wordCounters[extension] = wordCounter;
    }

    public IWordCounter? SelectWordCounter(string filePath)
    {
        var extension = Path.GetExtension(filePath)?.ToLower();
        if (extension != null && _wordCounters.TryGetValue(extension, out var wordCounter))
        {
            return wordCounter;
        }

        return null;
    }
}
