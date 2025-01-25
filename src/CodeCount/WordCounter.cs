using System;
using System.Collections.Generic;
using System.Linq;

public class WordCount
{
    public string Word { get; set; }
    public int Count { get; set; }
}

public class WordCounter
{
    public List<WordCount> GetWordCounts(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<WordCount>();
        }

        var wordCounts = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                             .GroupBy(word => word.ToLower())
                             .Select(group => new WordCount { Word = group.Key, Count = group.Count() })
                             .ToList();

        return wordCounts;
    }
}