public class WordCountAggregatorTests
{
    public class When_aggregating_word_counts
    {
        [Fact]
        public void Should_return_combined_word_counts_from_all_files()
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
    }
}