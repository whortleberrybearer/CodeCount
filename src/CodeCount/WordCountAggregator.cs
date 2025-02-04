public class WordCountAggregator
{
    private readonly FileSearcher _fileSearcher;
    private readonly WordCounter _wordCounter;

    public WordCountAggregator(FileSearcher fileSearcher, WordCounter wordCounter)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _wordCounter = wordCounter ?? throw new ArgumentNullException(nameof(wordCounter));
    }

    public WordCountResults AggregateWordCounts(string directoryPath, string[]? fileExtensions = null, int? maxResults = null)
    {
        var wordCountDictionary = new Dictionary<string, int>();
        var subDirectories = new List<WordCountResults>();

        var allFiles = _fileSearcher.GetAllFiles(directoryPath);
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

        var subdirectories = Directory.GetDirectories(directoryPath);
        foreach (var subdirectory in subdirectories)
        {
            var subdirectoryResult = AggregateWordCounts(subdirectory, fileExtensions, maxResults);
            subDirectories.Add(subdirectoryResult);
        }

        var results = wordCountDictionary.Select(kvp => new WordCountResult { Word = kvp.Key, Count = kvp.Value }).ToList();
        if (maxResults.HasValue)
        {
            results = results
                .OrderByDescending(result => result.Count)
                .ThenBy(result => result.Word)
                .Take(maxResults.Value)
                .ToList();
        }

        return new WordCountResults
        {
            Directory = directoryPath,
            WordCounts = results,
            SubDirectories = subDirectories
        };
    }
}