public class CSharpWordCounterTests
{
    // TODO: Ensure count is correct
    // TODO: Check variables with numbers
    // TODO: Check things not contained
    [Fact]
    public void GetWordCounts_ExcludesKeywords()
    {
        var counter = new CSharpWordCounter();
        var text = "public class TestClass { private int value; }";
        var wordCounts = counter.GetWordCounts(text);

        wordCounts.ShouldContainKeyAndValue("TestClass", 1);
        wordCounts.ShouldContainKeyAndValue("value", 1);
    }

    [Fact]
    public void GetWordCounts_IncludesKeywords_WhenExcludeKeywordsIsFalse()
    {
        var counter = new CSharpWordCounter { ExcludeKeywords = false };
        var text = "public class TestClass { private int value; }";
        var wordCounts = counter.GetWordCounts(text);

        wordCounts.ShouldContainKeyAndValue("public", 1);
        wordCounts.ShouldContainKeyAndValue("class", 1);
        wordCounts.ShouldContainKeyAndValue("TestClass", 1);
        wordCounts.ShouldContainKeyAndValue("private", 1);
        wordCounts.ShouldContainKeyAndValue("int", 1);
        wordCounts.ShouldContainKeyAndValue("value", 1);
    }

    [Fact]
    public void GetWordCounts_SplitsCamelAndPascalCase()
    {
        var counter = new CSharpWordCounter();
        var text = "public class TestClass { private int camelCaseVariable; }";
        var wordCounts = counter.GetWordCounts(text);

        wordCounts.ShouldContainKeyAndValue("Test", 1);
        wordCounts.ShouldContainKeyAndValue("Class", 1);
        wordCounts.ShouldContainKeyAndValue("camel", 1);
        wordCounts.ShouldContainKeyAndValue("Case", 1);
        wordCounts.ShouldContainKeyAndValue("Variable", 1);
    }

    [Fact]
    public void GetWordCounts_DoesNotSplitNames_WhenSplitNamesIsFalse()
    {
        var counter = new CSharpWordCounter { SplitNames = false };
        var text = "public class TestClass { private int camelCaseVariable; }";
        var wordCounts = counter.GetWordCounts(text);

        wordCounts.ShouldContainKeyAndValue("TestClass", 1);
        wordCounts.ShouldContainKeyAndValue("camelCaseVariable", 1);
    }
}
