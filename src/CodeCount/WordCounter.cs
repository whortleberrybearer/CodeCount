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
            var wordCounts = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                 .GroupBy(word => word.ToLower())
                                 .Select(group => new WordCountResult { Word = group.Key, Count = group.Count() })
                                 .ToList();

            return wordCounts;
        }
    }
}