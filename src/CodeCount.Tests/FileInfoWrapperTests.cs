public class FileInfoWrapperTests
{
    public class When_checking_an_extension
    {
        [Fact]
        public void Should_return_true_when_extension_is_valid()
        {
            var fileInfo = new FileInfo("test.txt");
            var validExtensions = new List<string> { ".txt", ".doc" };

            var wrapper = new FileInfoWrapper(fileInfo);

            var result = wrapper.HasValidExtension(validExtensions);

            result.ShouldBeTrue();
        }
        
        [Fact]
        public void Should_return_true_when_extension_is_not_valid()
        {
            var fileInfo = new FileInfo("test.exe");
            var validExtensions = new List<string> { ".txt", ".doc" };

            var wrapper = new FileInfoWrapper(fileInfo);

            var result = wrapper.HasValidExtension(validExtensions);

            result.ShouldBeFalse();
        }
    }

    public class When_getting_word_counts
    {
        [Fact]
        public void Should_return_word_counter_result()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                var fileInfo = new FileInfo(filePath);

                var wordCounter = new Mock<IWordCounter>();
                wordCounter.Setup(w => w.GetWordCounts(It.IsAny<Stream>())).Returns(new Dictionary<string, int> { { "test", 1 } });

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
