public class WordCounterTests
{
    public class When_counting_words
    {
        [Fact]
        public void An_empty_string_should_return_no_results()
        {
            var wordCounter = new WordCounter();

            var result = wordCounter.GetWordCounts(string.Empty);
            
            result.ShouldBeEmpty();
        }

        [Fact]
        public void A_single_word_should_return_one_result()
        {
            var wordCounter = new WordCounter();
            
            var results = wordCounter.GetWordCounts("hello");
            
            results.Count.ShouldBe(1);
            results.ShouldContainKeyAndValue("hello", 1);
        }

        [Fact]
        public void Duplicate_words_are_counted()
        {
            var wordCounter = new WordCounter();
            
            var results = wordCounter.GetWordCounts("hello world hello");
            
            results.Count.ShouldBe(2);
            results.ShouldContainKeyAndValue("hello", 2);
            results.ShouldContainKeyAndValue("world", 1);
        }

        [Fact]
        public void Case_should_be_ignored()
        {
            var wordCounter = new WordCounter();
            
            var results = wordCounter.GetWordCounts("Hello hello HELLO");
            
            results.Count.ShouldBe(1);
            results.ShouldContainKeyAndValue("hello", 3);
        }

        [Fact]
        public void Punctuation_and_numbers_should_be_ignored()
        {
            var wordCounter = new WordCounter();
            
            var results = wordCounter.GetWordCounts("Dave: Hello Steve1.\r\nSteve2: Hi! It's not Steve1.");
            
            results.Count.ShouldBe(6);
            results.ShouldContainKeyAndValue("dave", 1);
            results.ShouldContainKeyAndValue("hello", 1);
            results.ShouldContainKeyAndValue("steve", 3);
            results.ShouldContainKeyAndValue("hi", 1);
            results.ShouldContainKeyAndValue("it", 1);
            results.ShouldContainKeyAndValue("not", 1);
        }

        [Fact]
        public void Single_letters_should_be_ignored()
        {
            var wordCounter = new WordCounter();
            
            var results = wordCounter.GetWordCounts("a b c word");
            
            results.Count.ShouldBe(1);
            results.ShouldContainKeyAndValue("word", 1);
        }
    }
}

