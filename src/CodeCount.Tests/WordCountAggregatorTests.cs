public class WordCountAggregatorTests
{
    public class When_aggregating_word_counts
    {
        [Fact]
        public void Should_return_combined_word_counts_from_all_files()
        {
            var testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectoryPath);

            var file1Path = Path.Combine(testDirectoryPath, "file1.txt");
            var file2Path = Path.Combine(testDirectoryPath, "file2.txt");

            File.WriteAllText(file1Path, "hello world");
            File.WriteAllText(file2Path, "hello universe");

            var fileSearcher = new FileSearcher();
            var wordCounter = new WordCounter();
            var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

            try
            {
                var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

                results.Length.ShouldBe(3);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 2 });
                results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "universe", Count = 1 });
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }

        [Fact]
        public void Should_return_results_in_alphabetical_order()
        {
            var testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectoryPath);

            var file1Path = Path.Combine(testDirectoryPath, "file1.txt");
            var file2Path = Path.Combine(testDirectoryPath, "file2.txt");

            File.WriteAllText(file1Path, "banana apple cherry");
            File.WriteAllText(file2Path, "date fig grape");

            var fileSearcher = new FileSearcher();
            var wordCounter = new WordCounter();
            var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

            try
            {
                var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

                var expectedOrder = new[] { "apple", "banana", "cherry", "date", "fig", "grape" };
                var actualOrder = results.Select(r => r.Word).ToArray();

                actualOrder.ShouldBe(expectedOrder);
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }
    }
}