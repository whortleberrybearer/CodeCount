namespace CodeCount.Tests;

public class FileInfoWrapperTests
{
    public class When_getting_word_counts
    {
        [Fact]
        public void Should_return_word_counter_result()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                var fileInfo = new FileInfo(filePath);
                var text = "test";

                using (var streamWriter = fileInfo.CreateText())
                {
                    streamWriter.Write(text);
                }

                var wordCounter = new Mock<IWordCounter>();
                wordCounter.Setup(w => w.GetWordCounts(text)).Returns(new Dictionary<string, int> { { "test", 1 } });

                var wrapper = new FileInfoWrapper(fileInfo);

                var result = wrapper.GetWordCounts(wordCounter.Object);

                result.ShouldContainKeyAndValue("test", 1);
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
