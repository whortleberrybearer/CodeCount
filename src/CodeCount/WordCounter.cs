using System.Text.RegularExpressions;

public interface IWordCounter
{
    IDictionary<string, int> GetWordCounts(Stream stream);
}

public class WordCounter : IWordCounter
{
    public IDictionary<string, int> GetWordCounts(Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (stream.Length == 0)
        {
            return new Dictionary<string, int>();
        }

        using (var reader = new StreamReader(stream))
        {
            var text = reader.ReadToEnd();
            return Regex.Split(text, @"[^a-zA-Z]+")
                .Where(word => word.Length > 1) // Ignore single-letter words
                .GroupBy(word => word.ToLower())
                .ToDictionary(group => group.Key, group => group.Count());
        }
    }
}