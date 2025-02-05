public class WordCountAggregator
{
    private readonly IFileSearcher _fileSearcher;
    private readonly IWordCounterSelector _wordCounterSelector;

    public WordCountAggregator(IFileSearcher fileSearcher, IWordCounterSelector wordCounterSelector)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _wordCounterSelector = wordCounterSelector ?? throw new ArgumentNullException(nameof(wordCounterSelector));
    }

    public string[]? FileExtensions { get; set; }

    public int? MaxResults { get; set; }

    public IEnumerable<string>? ExcludedWords { get; set; }

    public IEnumerable<WordCountResult> AggregateWordCounts(string directoryPath)
    {
        var allFiles = _fileSearcher.GetAllFiles(directoryPath);
        var wordCountDictionary = new Dictionary<string, int>();

        foreach (var file in allFiles)
        {
            if (FileExtensions is not null && !file.HasValidExtension(FileExtensions))
            {
                Console.WriteLine($"Skipping file (invalid extension): {file.FullName}");
                continue;
            }

            var wordCounter = _wordCounterSelector.SelectWordCounter(file.FullName);

            if (wordCounter is null)
            {
                Console.WriteLine($"Skipping file (no word counter registered): {file.FullName}");
                continue;
            }

            Console.WriteLine($"Processing file: {file.FullName}");

            var wordCounts = file.GetWordCounts(wordCounter);

            foreach (var wordCount in wordCounts)
            {
                if (ExcludedWords is not null && ExcludedWords.Contains(wordCount.Key))
                {
                    continue;
                }

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

        if (MaxResults.HasValue)
        {
            results = results
                .OrderByDescending(result => result.Count)
                .ThenBy(result => result.Word)
                .Take(MaxResults.Value);
        }

        return results.OrderBy(result => result.Word);
    }
}