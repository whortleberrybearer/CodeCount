namespace CodeCount.Tests;

using System.Text.RegularExpressions;

public class WordCountAggregatorTests
{
    public class When_aggregating_word_counts
    {
        [Fact]
        public void Should_return_combined_word_counts_from_all_files()
        {
            var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());

            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounterSelector = new Mock<IWordCounterSelector>();
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

            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile1.Object.FullName))
                .Returns(mockWordCounter.Object);
            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile2.Object.FullName))
                .Returns(mockWordCounter.Object);

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object);

            var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

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
            var mockWordCounterSelector = new Mock<IWordCounterSelector>();
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

            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile1.Object.FullName))
                .Returns(mockWordCounter.Object);
            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile2.Object.FullName))
                .Returns(mockWordCounter.Object);

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object);

            var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

            var expectedOrder = new[] { "apple", "banana", "cherry", "date", "fig", "grape" };
            var actualOrder = results.Select(r => r.Word).ToArray();

            actualOrder.ShouldBe(expectedOrder);
        }

        [Fact]
        public void Should_ignore_files_when_no_counter_registered()
        {
            var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());

            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounterSelector = new Mock<IWordCounterSelector>();
            var mockWordCounter = new Mock<IWordCounter>();
            var mockFile1 = new Mock<IFileInfo>();
            var mockFile2 = new Mock<IFileInfo>();

            mockFile1
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "hello", 1 }, { "world", 1 } });
            mockFile1
                .Setup(f => f.FullName)
                .Returns("file1.txt");
            mockFile2
                .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                .Returns(new Dictionary<string, int> { { "hello", 1 }, { "universe", 1 } });
            mockFile2
                .Setup(f => f.FullName)
                .Returns("file2.txt");

            mockFileSearcher
                .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                .Returns(new[] { mockFile1.Object, mockFile2.Object });

            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile1.Object.FullName))
                .Returns(mockWordCounter.Object);
            mockWordCounterSelector
                .Setup(s => s.SelectWordCounter(mockFile2.Object.FullName))
                .Returns((WordCounter?)null);

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object);

            var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

            results.Length.ShouldBe(2);
            results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
            results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
        }

        public class And_excluded_words_specified
        {
            [Fact]
            public void Should_not_include_specified_words()
            {
                var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());

                var mockFileSearcher = new Mock<IFileSearcher>();
                var mockWordCounterSelector = new Mock<IWordCounterSelector>();
                var mockWordCounter = new Mock<IWordCounter>();
                var mockFile = new Mock<IFileInfo>();

                mockFile
                    .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                    .Returns(new Dictionary<string, int> { { "hello", 1 }, { "world", 1 }, { "exclude", 1 } });

                mockFileSearcher
                    .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                    .Returns(new[] { mockFile.Object });

                mockWordCounterSelector
                    .Setup(s => s.SelectWordCounter(mockFile.Object.FullName))
                    .Returns(mockWordCounter.Object);

                var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object)
                {
                    ExcludedWords = new[] { new Regex("exclude", RegexOptions.IgnoreCase) }
                };

                var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

                results.Length.ShouldBe(2);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
                results.ShouldNotContain(r => r.Word == "exclude");
            }

            [Fact]
            public void Should_not_include_words_matching_regex_pattern()
            {
                var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());

                var mockFileSearcher = new Mock<IFileSearcher>();
                var mockWordCounterSelector = new Mock<IWordCounterSelector>();
                var mockWordCounter = new Mock<IWordCounter>();
                var mockFile = new Mock<IFileInfo>();

                mockFile
                    .Setup(f => f.GetWordCounts(mockWordCounter.Object))
                    .Returns(new Dictionary<string, int> { { "hello", 1 }, { "world", 1 }, { "exclude1", 1 }, { "exclude2", 1 } });

                mockFileSearcher
                    .Setup(fs => fs.GetAllFiles(testDirectoryPath))
                    .Returns(new[] { mockFile.Object });

                mockWordCounterSelector
                    .Setup(s => s.SelectWordCounter(mockFile.Object.FullName))
                    .Returns(mockWordCounter.Object);

                var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object)
                {
                    ExcludedWords = new[] { new Regex("exclude\\d", RegexOptions.IgnoreCase) }
                };

                var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

                results.Length.ShouldBe(2);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
                results.ShouldNotContain(r => r.Word.StartsWith("exclude"));
            }
        }

        public class And_max_results_specified
        {
            [Fact]
            public void Should_return_highest_count_words_in_alphabetical_order()
            {
                var testDirectoryPath = Path.Combine("test", Guid.NewGuid().ToString());

                var mockFileSearcher = new Mock<IFileSearcher>();
                var mockWordCounterSelector = new Mock<IWordCounterSelector>();
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

                mockWordCounterSelector
                    .Setup(s => s.SelectWordCounter(mockFile1.Object.FullName))
                    .Returns(mockWordCounter.Object);
                mockWordCounterSelector
                    .Setup(s => s.SelectWordCounter(mockFile2.Object.FullName))
                    .Returns(mockWordCounter.Object);

                var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounterSelector.Object)
                {
                    MaxResults = 2
                };

                var results = aggregator.AggregateWordCounts(testDirectoryPath).ToArray();

                var expectedOrder = new[] { "hello", "world" };
                var actualOrder = results.Select(r => r.Word).ToArray();

                actualOrder.ShouldBe(expectedOrder);
            }
        }
    }
}