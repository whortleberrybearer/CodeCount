using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class WordCounterTests
{
    [TestMethod]
    public void TestEmptyString()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts(string.Empty);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void TestSingleWord()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello");
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("hello", result[0].Word);
        Assert.AreEqual(1, result[0].Count);
    }

    [TestMethod]
    public void TestMultipleWords()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello world hello");
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("hello", result[0].Word);
        Assert.AreEqual(2, result[0].Count);
        Assert.AreEqual("world", result[1].Word);
        Assert.AreEqual(1, result[1].Count);
    }

    [TestMethod]
    public void TestCaseInsensitive()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("Hello hello HELLO");
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("hello", result[0].Word);
        Assert.AreEqual(3, result[0].Count);
    }

    [TestMethod]
    public void TestPunctuation()
    {
        var wordCounter = new WordCounter();
        var result = wordCounter.GetWordCounts("hello, world! hello.");
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("hello", result[0].Word);
        Assert.AreEqual(2, result[0].Count);
        Assert.AreEqual("world", result[1].Word);
        Assert.AreEqual(1, result[1].Count);
    }
}