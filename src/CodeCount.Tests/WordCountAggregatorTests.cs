using Moq;

public class WordCountAggregatorTests
{
    public class When_aggregating_word_counts
    {
        [Fact]
        public void Should_return_combined_word_counts_from_all_files()
        {
            var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());
            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounter = new Mock<IWordCounter>();
            var mockFile1 = new Mock<IFileInfo>();
            var mockFile2 = new Mock<IFileInfo>();

            mockFile1
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "hello", 1 }, { "world", 1 } });
            mockFile2
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "hello", 1 }, { "universe", 1 } });

            mockFileSearcher
                .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                .Returns(new[] { mockFile1.Object, mockFile2.Object });

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

            var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" }).ToArray();

            results.Length.ShouldBe(3);
            results.ShouldContain(new WordCountResult() { Word = "hello", Count = 2 });
            results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
            results.ShouldContain(new WordCountResult() { Word = "universe", Count = 1 });
        }

        [Fact]
        public void Should_return_results_in_alphabetical_order()
        {
            var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());
            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounter = new Mock<IWordCounter>();
            var mockFile1 = new Mock<IFileInfo>();
            var mockFile2 = new Mock<IFileInfo>();

            mockFile1
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "banana", 1 }, { "apple", 1 }, { "cherry", 1 }, { "grape", 1 } });
            mockFile2
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "date", 1 }, { "fig", 1 }, { "grape", 1 } });

            mockFileSearcher
                .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                .Returns(new[] { mockFile1.Object, mockFile2.Object });

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

            var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" }).ToArray();

            var expectedOrder = new[] { "apple", "banana", "cherry", "date", "fig", "grape" };
            var actualOrder = results.Select(r => r.Word).ToArray();

            actualOrder.ShouldBe(expectedOrder);
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
                var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" }).ToArray();

                results.Length.ShouldBe(2);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
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
                var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());
                var mockFileSearcher = new Mock<IFileSearcher>();
                var mockWordCounter = new Mock<IWordCounter>();
                var mockFile1 = new Mock<IFileInfo>();
                var mockFile2 = new Mock<IFileInfo>();

                mockFile1
                    .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                    .Returns(new Dictionary<string, int> { { "hello", 2 }, { "world", 2 } });
                mockFile2
                    .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                    .Returns(new Dictionary<string, int> { { "hello", 1 }, { "universe", 1 } });

                mockFileSearcher
                    .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                    .Returns(new[] { mockFile1.Object, mockFile2.Object });

                var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

                var results = aggregator.AggregateWordCounts(testDirectoryPath, new[] { ".txt" }, 2).ToArray();

                var expectedOrder = new[] { "hello", "world" };
                var actualOrder = results.Select(r => r.Word).ToArray();

                actualOrder.ShouldBe(expectedOrder);
            }
        }
    }
}