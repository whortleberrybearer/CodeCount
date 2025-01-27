public record WordCountResult
{
    public required string Word { get; init; }

    public int Count { get; internal set; }
}