using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Shouldly;

public class WordCountAggregatorTests
{
    [Fact]
    public void AggregateWordCounts_ShouldReturnCombinedWordCountsFromAllFiles()
    {
        // Arrange
        var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);

        var file1 = Path.Combine(testDirectory, "file1.txt");
        var file2 = Path.Combine(testDirectory, "file2.txt");

        File.WriteAllText(file1, "hello world");
        File.WriteAllText(file2, "hello universe");

        var fileSearcher = new FileSearcher();
        var wordCounter = new WordCounter();
        var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

        try
        {
            // Act
            var result = aggregator.AggregateWordCounts(testDirectory).ToArray();

            // Assert
            result.Length.ShouldBe(3);
            result.ShouldContain(r => r.Word == "hello" && r.Count == 2);
            result.ShouldContain(r => r.Word == "world" && r.Count == 1);
            result.ShouldContain(r => r.Word == "universe" && r.Count == 1);
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDirectory, true);
        }
    }

    [Fact]
    public void AggregateWordCounts_ShouldThrowArgumentException_WhenDirectoryPathIsNullOrEmpty()
    {
        var fileSearcher = new FileSearcher();
        var wordCounter = new WordCounter();
        var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

        Should.Throw<ArgumentException>(() => aggregator.AggregateWordCounts(null));
        Should.Throw<ArgumentException>(() => aggregator.AggregateWordCounts(string.Empty));
    }

    [Fact]
    public void AggregateWordCounts_ShouldThrowDirectoryNotFoundException_WhenDirectoryDoesNotExist()
    {
        var fileSearcher = new FileSearcher();
        var wordCounter = new WordCounter();
        var aggregator = new WordCountAggregator(fileSearcher, wordCounter);
        var nonExistentDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Should.Throw<DirectoryNotFoundException>(() => aggregator.AggregateWordCounts(nonExistentDirectory));
    }
}