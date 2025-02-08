public record Config
{
    public required string SourceDirectoryPath { get; init; }
    public required string OutputFilePath { get; init; }
    public int? MaxResults { get; init; }
    public string[]? ValidFileExtensions { get; init; }
    public string[]? ExcludeFilter { get; init; }
    public string[]? ExcludeWords { get; init; }
    public List<WordCounterConfig>? WordCounters { get; init; }
}

public record WordCounterConfig
{
    public required string Type { get; init; }
    public required string[] Filters { get; init; }
    public Dictionary<string, object>? Properties { get; init; }
}