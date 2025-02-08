public class WordCounterFactoryTests
{
    public class When_creating_a_word_counter
    {
        [Fact]
        public void Should_throw_an_exception_when_the_type_is_not_found()
        {
            var config = new WordCounterConfig
            {
                Type = "Invalid.Type",
                Filters = new[] { "**/*" }
            };

            var factory = new WordCounterFactory();

            Should.Throw<InvalidOperationException>(() => factory.CreateWordCounter(config));
        }

        [Fact]
        public void Should_create_word_counter_and_set_properties()
        {
            var config = new WordCounterConfig
            {
                Type = "WordCounter",
                Filters = new[] { "**/*" }
            };
            var factory = new WordCounterFactory();

            var wordCounter = factory.CreateWordCounter(config);

            wordCounter.ShouldNotBeNull();
            wordCounter.ShouldBeOfType<WordCounter>();
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Should_create_csharp_word_counter_and_set_properties(bool excludeKeywords, bool splitNames)
        {
            var config = new WordCounterConfig
            {
                Type = "CSharpWordCounter",
                Filters = new[] { "**/*" },
                Properties = new Dictionary<string, object>
                {
                    { "ExcludeKeywords", excludeKeywords },
                    { "SplitNames", splitNames }
                }
            };

            var factory = new WordCounterFactory();

            var wordCounter = factory.CreateWordCounter(config);

            wordCounter.ShouldNotBeNull();
            wordCounter.ShouldBeOfType<CSharpWordCounter>();
            
            var csharpWordCounter = (CSharpWordCounter)wordCounter;
            csharpWordCounter.ExcludeKeywords.ShouldBe(excludeKeywords);
            csharpWordCounter.SplitNames.ShouldBe(splitNames);
        }
    }
}
