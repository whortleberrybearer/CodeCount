using System.Text.RegularExpressions;

public interface IWordCounter
{
    IDictionary<string, int> GetWordCounts(string text);
}

public class WordCounter : IWordCounter
{
    public IDictionary<string, int> GetWordCounts(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (text.Length == 0)
        {
            return new Dictionary<string, int>();
        }

        return Regex.Split(text, @"[^a-zA-Z]+")
            .Where(word => word.Length > 1) // Ignore single-letter words
            .GroupBy(word => word.ToLower())
            .ToDictionary(group => group.Key, group => group.Count());
    }
}