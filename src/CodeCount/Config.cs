public record Config
{
    public required string SourceDirectoryPath { get; init; }

    public required string OutputFilePath { get; init; }

    public int? MaxResults { get; init; } 
}