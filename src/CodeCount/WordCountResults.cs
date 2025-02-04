public class WordCountResults
{
    public required string Directory { get; init; }
    public IEnumerable<WordCountResult> WordCounts { get; internal set; } = Enumerable.Empty<WordCountResult>();
    public IEnumerable<WordCountResults> SubDirectories { get; internal set; } = Enumerable.Empty<WordCountResults>();
}
