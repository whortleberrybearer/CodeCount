public class WordCountAggregator
{
    private readonly IFileSearcher _fileSearcher;
    private readonly IWordCounter _wordCounter;

    public WordCountAggregator(IFileSearcher fileSearcher, IWordCounter wordCounter)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _wordCounter = wordCounter ?? throw new ArgumentNullException(nameof(wordCounter));
    }

    public IEnumerable<WordCountResult> AggregateWordCounts(string directoryPath, string[]? fileExtensions = null, int? maxResults = null)
    {
        var allFiles = _fileSearcher.GetAllFiles(directoryPath);
        var wordCountDictionary = new Dictionary<string, int>();

        foreach (var file in allFiles)
        {
            if (fileExtensions is not null && !file.HasValidExtension(fileExtensions))
            {
                continue;
            }

            var wordCounts = file.GetWordCounts(_wordCounter);

            foreach (var wordCount in wordCounts)
            {
                if (wordCountDictionary.ContainsKey(wordCount.Key))
                {
                    wordCountDictionary[wordCount.Key] += wordCount.Value;
                }
                else
                {
                    wordCountDictionary[wordCount.Key] = wordCount.Value;
                }
            }
        }

        var results = wordCountDictionary.Select(kvp => new WordCountResult { Word = kvp.Key, Count = kvp.Value });

        if (maxResults.HasValue)
        {
            results = results
                .OrderByDescending(result => result.Count)
                .ThenBy(result => result.Word)
                .Take(maxResults.Value);
        }

        return results.OrderBy(result => result.Word);
    }
}