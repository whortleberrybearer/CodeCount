using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WordCount
{
    public string Word { get; set; }
    public int Count { get; set; }
}

public class WordCounter
{
    public IEnumerable<WordCount> GetWordCounts(Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (stream.Length == 0)
        {
            return Enumerable.Empty<WordCount>();
        }

        using (var reader = new StreamReader(stream))
        {
            var text = reader.ReadToEnd();
            var wordCounts = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                 .GroupBy(word => word.ToLower())
                                 .Select(group => new WordCount { Word = group.Key, Count = group.Count() })
                                 .ToList();

            return wordCounts;
        }
    }
}