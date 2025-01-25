using System.IO;

public class WordCounterTests
{
    private Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    [Fact]
    public void TestEmptyString()
    {
        var wordCounter = new WordCounter();
        using (var stream = GenerateStreamFromString(string.Empty))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();
            result.Length.ShouldBe(0);
        }
    }

    [Fact]
    public void TestSingleWord()
    {
        var wordCounter = new WordCounter();
        using (var stream = GenerateStreamFromString("hello"))
        {
            var result = wordCounter.GetWordCounts(stream).ToArray();
            result.Length.ShouldBe(1);
            result[0].Word.ShouldBe("hello");
            result[0].Count.ShouldBe(1);
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