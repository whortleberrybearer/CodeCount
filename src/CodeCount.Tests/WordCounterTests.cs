public class WordCounterTests
{
    public class When_counting_words
    {
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            
            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                writer.Write(s);
                writer.Flush();
            }

            stream.Position = 0;

            return stream;
        }

        [Fact]
        public void An_empty_string_should_return_no_results()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString(string.Empty))
            {
                var result = wordCounter.GetWordCounts(stream);

                result.ShouldBeEmpty();
            }
        }

        [Fact]
        public void A_single_word_should_return_one_result()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("hello"))
            {
                var results = wordCounter.GetWordCounts(stream);

                results.Count.ShouldBe(1);
                results.ShouldContainKeyAndValue("hello", 1);
            }
        }

        [Fact]
        public void Duplicate_words_are_counted()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("hello world hello"))
            {
                var results = wordCounter.GetWordCounts(stream);
                
                results.Count.ShouldBe(2);
                results.ShouldContainKeyAndValue("hello", 2);
                results.ShouldContainKeyAndValue("world", 1);
            }
        }

        [Fact]
        public void Case_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("Hello hello HELLO"))
            {
                var results = wordCounter.GetWordCounts(stream);

                results.Count.ShouldBe(1);
                results.ShouldContainKeyAndValue("hello", 3);
            }
        }

        [Fact]
        public void Punctuation_and_numbers_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("Dave: Hello Steve1.\r\nSteve2: Hi! It's not Steve1."))
            {
                var results = wordCounter.GetWordCounts(stream);

                results.Count.ShouldBe(6);
                results.ShouldContainKeyAndValue("dave", 1);
                results.ShouldContainKeyAndValue("hello", 1);
                results.ShouldContainKeyAndValue("steve", 3);
                results.ShouldContainKeyAndValue("hi", 1);
                results.ShouldContainKeyAndValue("it", 1);
                results.ShouldContainKeyAndValue("not", 1);
            }
        }

        [Fact]
        public void Single_letters_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("a b c word"))
            {
                var results = wordCounter.GetWordCounts(stream);

                results.Count.ShouldBe(1);
                results.ShouldContainKeyAndValue("word", 1);
            }
        }
    }
}

