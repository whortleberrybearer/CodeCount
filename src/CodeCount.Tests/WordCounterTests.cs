public class WordCounterTests
{
    private Stream GenerateStreamFromString(string s)
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
    public void Empty_string_should_return_no_results()
    {
        var wordCounter = new WordCounter();

        using (var stream = GenerateStreamFromString(string.Empty))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();

            result.ShouldBeEmpty();
        }
    }

    [Fact]
    public void Single_word_should_return_one_result()
    {
        var wordCounter = new WordCounter();

        using (var stream = GenerateStreamFromString("hello"))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();

            result.Length.ShouldBe(1);
            result[0].ShouldBe(
                new WordCountResult() { Word = "hello", Count = 1 },
                new WordCountResultComparer());

            //result.Length.ShouldBe(1);
            //result[0].Word.ShouldBe("hello");
            //result[0].Count.ShouldBe(1);
        }
    }

    [Fact]
    public void TestMultipleWords()
    {
        var wordCounter = new WordCounter();

        using (var stream = GenerateStreamFromString("hello world hello"))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();
            result.Length.ShouldBe(2);
            result[0].Word.ShouldBe("hello");
            result[0].Count.ShouldBe(2);
            result[1].Word.ShouldBe("world");
            result[1].Count.ShouldBe(1);
        }
    }

    [Fact]
    public void TestCaseInsensitive()
    {
        var wordCounter = new WordCounter();

        using (var stream = GenerateStreamFromString("Hello hello HELLO"))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();
            result.Length.ShouldBe(1);
            result[0].Word.ShouldBe("hello");
            result[0].Count.ShouldBe(3);
        }
    }

    [Fact]
    public void TestPunctuation()
    {
        var wordCounter = new WordCounter();

        using (var stream = GenerateStreamFromString("hello, world! hello."))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();
            result.Length.ShouldBe(2);
            result[0].Word.ShouldBe("hello");
            result[0].Count.ShouldBe(2);
            result[1].Word.ShouldBe("world");
            result[1].Count.ShouldBe(1);
        }
    }
}

