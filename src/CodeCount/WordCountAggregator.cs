using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WordCountAggregator
{
    private readonly FileSearcher _fileSearcher;
    private readonly WordCounter _wordCounter;

    public WordCountAggregator(FileSearcher fileSearcher, WordCounter wordCounter)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _wordCounter = wordCounter ?? throw new ArgumentNullException(nameof(wordCounter));
    }

    public IEnumerable<WordCountResult> AggregateWordCounts(string directoryPath)
    {
        var allFiles = _fileSearcher.GetAllFiles(directoryPath);
        var wordCountDictionary = new Dictionary<string, int>();

        foreach (var file in allFiles)
        {
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

        return wordCountDictionary.Select(kvp => new WordCountResult { Word = kvp.Key, Count = kvp.Value })
                                  .OrderBy(result => result.Word);
    }
}