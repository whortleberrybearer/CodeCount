using System.Text.RegularExpressions;

public interface IWordCounter
{
    IDictionary<string, int> GetWordCounts(string text);
}

public class WordCounter : IWordCounter
{
    public WordCounter() 
        : this(new Regex(@"[^a-zA-Z0-9]+"))
    {
    }

    protected WordCounter(Regex splitExpression)
    {
        SplitExpression = splitExpression;
    }

    public Regex SplitExpression { get; set; }

    public virtual IDictionary<string, int> GetWordCounts(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (text.Length == 0)
        {
            return new Dictionary<string, int>();
        }

        return SplitText(text)
            .Where(word => word.Length > 1) // Ignore single-letter words
            .GroupBy(word => word.ToLower())
            .ToDictionary(group => group.Key, group => group.Count());
    }

    protected virtual IEnumerable<string> SplitText(string text)
    {
        return SplitExpression.Split(text);
    }
}