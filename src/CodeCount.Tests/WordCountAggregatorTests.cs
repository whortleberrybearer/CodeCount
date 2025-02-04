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

            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounter = new Mock<IWordCounter>();
            var mockFile1 = new Mock<IFileInfo>();
            var mockFile2 = new Mock<IFileInfo>();

            mockFile1.Setup(f => f.FullName).Returns(file1Path);
            mockFile1.Setup(f => f.OpenRead()).Returns(() => new FileStream(file1Path, FileMode.Open, FileAccess.Read));
            mockFile2.Setup(f => f.FullName).Returns(file2Path);
            mockFile2.Setup(f => f.OpenRead()).Returns(() => new FileStream(file2Path, FileMode.Open, FileAccess.Read));

            mockFileSearcher.Setup(fs => fs.GetAllFiles(testDirectoryPath)).Returns(new[] { mockFile1.Object, mockFile2.Object });
            mockWordCounter.Setup(wc => wc.GetWordCounts(It.IsAny<Stream>())).Returns<Stream>(stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    if (text == "hello world")
                    {
                        return new Dictionary<string, int> { { "hello", 1 }, { "world", 1 } };
                    }
                    else if (text == "hello universe")
                    {
                        return new Dictionary<string, int> { { "hello", 1 }, { "universe", 1 } };
                    }
                    return new Dictionary<string, int>();
                }
            });

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

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

            File.WriteAllText(file1Path, "banana apple cherry grape");
            File.WriteAllText(file2Path, "date fig grape");

            var mockFileSearcher = new Mock<IFileSearcher>();
            var mockWordCounter = new Mock<IWordCounter>();
            var mockFile1 = new Mock<IFileInfo>();
            var mockFile2 = new Mock<IFileInfo>();

            mockFile1.Setup(f => f.FullName).Returns(file1Path);
            mockFile1.Setup(f => f.OpenRead()).Returns(() => new FileStream(file1Path, FileMode.Open, FileAccess.Read));
            mockFile2.Setup(f => f.FullName).Returns(file2Path);
            mockFile2.Setup(f => f.OpenRead()).Returns(() => new FileStream(file2Path, FileMode.Open, FileAccess.Read));

            mockFileSearcher.Setup(fs => fs.GetAllFiles(testDirectoryPath)).Returns(new[] { mockFile1.Object, mockFile2.Object });
            mockWordCounter.Setup(wc => wc.GetWordCounts(It.IsAny<Stream>())).Returns<Stream>(stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    if (text == "banana apple cherry grape")
                    {
                        return new Dictionary<string, int> { { "banana", 1 }, { "apple", 1 }, { "cherry", 1 }, { "grape", 1 } };
                    }
                    else if (text == "date fig grape")
                    {
                        return new Dictionary<string, int> { { "date", 1 }, { "fig", 1 }, { "grape", 1 } };
                    }
                    return new Dictionary<string, int>();
                }
            });

            var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

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

                var mockFileSearcher = new Mock<IFileSearcher>();
                var mockWordCounter = new Mock<IWordCounter>();
                var mockFile1 = new Mock<IFileInfo>();
                var mockFile2 = new Mock<IFileInfo>();

                mockFile1.Setup(f => f.FullName).Returns(file1);
                mockFile1.Setup(f => f.OpenRead()).Returns(() => new FileStream(file1, FileMode.Open, FileAccess.Read));
                mockFile2.Setup(f => f.FullName).Returns(file2);
                mockFile2.Setup(f => f.OpenRead()).Returns(() => new FileStream(file2, FileMode.Open, FileAccess.Read));

                mockFileSearcher.Setup(fs => fs.GetAllFiles(testDirectory)).Returns(new[] { mockFile1.Object, mockFile2.Object });
                mockWordCounter.Setup(wc => wc.GetWordCounts(It.IsAny<Stream>())).Returns<Stream>(stream =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = reader.ReadToEnd();
                        if (text == "hello world hello world")
                        {
                            return new Dictionary<string, int> { { "hello", 2 }, { "world", 2 } };
                        }
                        else if (text == "universe world")
                        {
                            return new Dictionary<string, int> { { "universe", 1 }, { "world", 1 } };
                        }
                        return new Dictionary<string, int>();
                    }
                });

                var aggregator = new WordCountAggregator(mockFileSearcher.Object, mockWordCounter.Object);

                try
                {
                    var results = aggregator.AggregateWordCounts(testDirectory, 2).ToArray();

                    var expectedOrder = new[] { "hello", "world" };
                    var actualOrder = results.Select(r => r.Word).ToArray();

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