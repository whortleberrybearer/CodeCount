using System.Text.RegularExpressions;

public class WordCounter
{
    public IEnumerable<WordCountResult> GetWordCounts(Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (stream.Length == 0)
        {
            return Enumerable.Empty<WordCountResult>();
        }

        using (var reader = new StreamReader(stream))
        {
            var text = reader.ReadToEnd();
            
            return Regex.Split(text, @"[^a-zA-Z]+")
                .Where(word => word.Length > 1) // Ignore single-letter words
                .GroupBy(word => word.ToLower())
                .Select(group => new WordCountResult { Word = group.Key, Count = group.Count() })
                .OrderBy(result => result.Word) // Ensure results are in alphabetical order
                .ToArray();
        }
    }
}