using System;
using System.Collections.Generic;
using Xunit;
using Shouldly;

public class WordCounterTests
{
    [Fact]
    public void TestEmptyString()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts(string.Empty);
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void TestSingleWord()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello");
        result.Count.ShouldBe(1);
        result[0].Word.ShouldBe("hello");
        result[0].Count.ShouldBe(1);
    }

    [Fact]
    public void TestMultipleWords()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello world hello");
        result.Count.ShouldBe(2);
        result[0].Word.ShouldBe("hello");
        result[0].Count.ShouldBe(2);
        result[1].Word.ShouldBe("world");
        result[1].Count.ShouldBe(1);
    }

    [Fact]
    public void TestCaseInsensitive()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("Hello hello HELLO");
        result.Count.ShouldBe(1);
        result[0].Word.ShouldBe("hello");
        result[0].Count.ShouldBe(3);
    }

    [Fact]
    public void TestPunctuation()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello, world! hello.");
        result.Count.ShouldBe(2);
        result[0].Word.ShouldBe("hello");
        result[0].Count.ShouldBe(2);
        result[1].Word.ShouldBe("world");
        result[1].Count.ShouldBe(1);
    }
}