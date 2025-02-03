public class WordCountAggregator
{
    private readonly FileSearcher _fileSearcher;
    private readonly WordCounter _wordCounter;

    public WordCountAggregator(FileSearcher fileSearcher, WordCounter wordCounter)
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
            if (fileExtensions is not null && !fileExtensions.Contains(Path.GetExtension(file)))
            {
                continue;
            }

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var wordCounts = _wordCounter.GetWordCounts(stream);

                foreach (var wordCount in wordCounts)
                {
                    if (wordCountDictionary.ContainsKey(wordCount.Word))
                    {
                        wordCountDictionary[wordCount.Word] += wordCount.Count;
                    }
                    else
                    {
                        wordCountDictionary[wordCount.Word] = wordCount.Count;
                    }
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