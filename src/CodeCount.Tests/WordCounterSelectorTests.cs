public class WordCounterSelectorTests
{
    public class When_selecting_a_word_counter
    {
        [Fact]
        public void Should_return_null_if_no_word_counter_registered()
        {
            var selector = new WordCounterSelector();
            var mockWordCounter = new Mock<IWordCounter>();

            selector.RegisterWordCounter(new[] { "**/*.doc" }, mockWordCounter.Object);

            var selectedCounter = selector.SelectWordCounter("file.txt");

            selectedCounter.ShouldBeNull();
        }

        [Fact]
        public void Should_return_counter_registered_for_filter()
        {
            var selector = new WordCounterSelector();
            var mockWordCounter = new Mock<IWordCounter>();

            selector.RegisterWordCounter(new[] { "**/*.txt" }, mockWordCounter.Object);

            var selectedCounter = selector.SelectWordCounter("file.txt");

            selectedCounter.ShouldBe(mockWordCounter.Object);
        }
    }
}
