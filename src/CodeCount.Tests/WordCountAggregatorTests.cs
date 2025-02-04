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
                var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" });

                results.SubDirectories.ShouldBeEmpty();
                results.Directory.ShouldBe(testDirectoryPath);
                results.WordCounts.ShouldContain(new WordCountResult { Word = "hello", Count = 2 });
                results.WordCounts.ShouldContain(new WordCountResult { Word = "world", Count = 1 });
                results.WordCounts.ShouldContain(new WordCountResult { Word = "universe", Count = 1 });
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

            File.WriteAllText(file1Path, "banana apple cherry grape");
            File.WriteAllText(file2Path, "date fig grape");

            var fileSearcher = new FileSearcher();
            var wordCounter = new WordCounter();
            var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

            try
            {
                var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" });

                var expectedOrder = new[] { "apple", "banana", "cherry", "date", "fig", "grape" };
                var actualOrder = results.WordCounts.Select(r => r.Word).ToArray();

                actualOrder.ShouldBe(expectedOrder);
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }

        [Fact]
        public void Should_ignore_files_with_different_extensions()
        {
            var testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectoryPath);

            var file1Path = Path.Combine(testDirectoryPath, "file1.txt");
            var file2Path = Path.Combine(testDirectoryPath, "file2.md");

            File.WriteAllText(file1Path, "hello world");
            File.WriteAllText(file2Path, "hello universe");

            var fileSearcher = new FileSearcher();
            var wordCounter = new WordCounter();
            var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

            try
            {
                var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" });

                results.WordCounts.Count().ShouldBe(2);
                results.WordCounts.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
                results.WordCounts.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }

        public class And_max_results_specified
        {
            [Fact]
            public void Should_return_highest_count_words_in_alphabetical_order()
            {
                var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(testDirectory);

                var file1 = Path.Combine(testDirectory, "file1.txt");
                var file2 = Path.Combine(testDirectory, "file2.txt");

                File.WriteAllText(file1, "hello world hello world");
                File.WriteAllText(file2, "universe world");

                var fileSearcher = new FileSearcher();
                var wordCounter = new WordCounter();
                var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

                try
                {
                    var results = aggregator.AggregateWordCounts(testDirectory, new[] { ".txt" }, 2);

                    var expectedOrder = new[] { "hello", "world" };
                    var actualOrder = results.WordCounts.Select(r => r.Word).ToArray();

                    actualOrder.ShouldBe(expectedOrder);
                }
                finally
                {
                    Directory.Delete(testDirectory, true);
                }
            }
        }
    }
}