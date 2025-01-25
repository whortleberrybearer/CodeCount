using System;
using System.Collections.Generic;
using Xunit;

public class WordCounterTests
{
    [Fact]
    public void TestEmptyString()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts(string.Empty);
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public void TestSingleWord()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello");
        Assert.Single(result);
        Assert.Equal("hello", result[0].Word);
        Assert.Equal(1, result[0].Count);
    }

    [Fact]
    public void TestMultipleWords()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello world hello");
        Assert.Equal(2, result.Count);
        Assert.Equal("hello", result[0].Word);
        Assert.Equal(2, result[0].Count);
        Assert.Equal("world", result[1].Word);
        Assert.Equal(1, result[1].Count);
    }

    [Fact]
    public void TestCaseInsensitive()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("Hello hello HELLO");
        Assert.Single(result);
        Assert.Equal("hello", result[0].Word);
        Assert.Equal(3, result[0].Count);
    }

    [Fact]
    public void TestPunctuation()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello, world! hello.");
        Assert.Equal(2, result.Count);
        Assert.Equal("hello", result[0].Word);
        Assert.Equal(2, result[0].Count);
        Assert.Equal("world", result[1].Word);
        Assert.Equal(1, result[1].Count);
    }
}