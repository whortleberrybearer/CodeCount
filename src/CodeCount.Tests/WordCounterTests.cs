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
                var result = wordCounter.GetWordCounts(stream).ToArray();

                result.ShouldBeEmpty();
            }
        }

        [Fact]
        public void A_single_word_should_return_one_result()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("hello"))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();

                results.Length.ShouldBe(1);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
            }
        }

        [Fact]
        public void Duplicate_words_counted()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("hello world hello"))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();
                
                results.Length.ShouldBe(2);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 2 });
                results.ShouldContain(new WordCountResult() { Word = "world", Count = 1 });
            }
        }

        [Fact]
        public void Case_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("Hello hello HELLO"))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();

                results.Length.ShouldBe(1);
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 3 });
            }
        }

        [Fact]
        public void Punctuation_and_numbers_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("Dave: Hello Steve1.\r\nSteve2: Hi! It's not Steve1."))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();

                results.Length.ShouldBe(6);
                results.ShouldContain(new WordCountResult() { Word = "dave", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "hello", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "steve", Count = 3 });
                results.ShouldContain(new WordCountResult() { Word = "hi", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "it", Count = 1 });
                results.ShouldContain(new WordCountResult() { Word = "not", Count = 1 });
            }
        }

        [Fact]
        public void Results_should_be_in_alphabetical_order()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("The quick brown fox"))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();

                results.Length.ShouldBe(4);
                results[0].Word.ShouldBe("brown");
                results[1].Word.ShouldBe("fox");
                results[2].Word.ShouldBe("quick");
                results[3].Word.ShouldBe("the");
            }
        }

        [Fact]
        public void Single_letters_should_be_ignored()
        {
            var wordCounter = new WordCounter();

            using (var stream = GenerateStreamFromString("a b c word"))
            {
                var results = wordCounter.GetWordCounts(stream).ToArray();

                results.Count().ShouldBe(1);
                results.ShouldContain(new WordCountResult() { Word = "word", Count = 1 });
            }
        }
    }
}

