namespace CodeCount.Tests;

public class CSharpWordCounterTests
{
    public class When_counting_words
    {
        [Fact]
        public void Camel_and_pascal_names_should_be_split()
        {
            var counter = new CSharpWordCounter();

            var wordCounts = counter.GetWordCounts("public class TestClass { private int camelCaseVariable; }");

            wordCounts.Count.ShouldBe(5);
            wordCounts.ShouldContainKeyAndValue("test", 1);
            wordCounts.ShouldContainKeyAndValue("class", 1);
            wordCounts.ShouldContainKeyAndValue("camel", 1);
            wordCounts.ShouldContainKeyAndValue("case", 1);
            wordCounts.ShouldContainKeyAndValue("variable", 1);
        }

        [Fact]
        public void Keywords_should_be_excluded()
        {
            var counter = new CSharpWordCounter();

            var wordCounts = counter.GetWordCounts("public class TestClass { private int value; }");

            // Although class is a keyword, it should still be counted as it has been split form TestClass,
            wordCounts.Count.ShouldBe(2);
            wordCounts.ShouldContainKeyAndValue("test", 1);
            wordCounts.ShouldContainKeyAndValue("class", 1);
        }

        [Fact]
        public void Numbers_should_be_ignored()
        {
            var counter = new CSharpWordCounter();

            var wordCounts = counter.GetWordCounts("public class TestClass { private int value123; int value456; }");

            wordCounts.ShouldContainKeyAndValue("value", 2);
            wordCounts.ShouldNotContainKey("value123");
            wordCounts.ShouldNotContainKey("value456");
        }

        public class When_keywords_are_not_excluded
        {
            [Fact]
            public void Keywords_should_be_included()
            {
                var counter = new CSharpWordCounter { ExcludeKeywords = false };

                var wordCounts = counter.GetWordCounts("public class TestClass { private int value; }");

                wordCounts.Count.ShouldBe(6);
                wordCounts.ShouldContainKeyAndValue("public", 1);
                wordCounts.ShouldContainKeyAndValue("class", 2);
                wordCounts.ShouldContainKeyAndValue("test", 1);
                wordCounts.ShouldContainKeyAndValue("private", 1);
                wordCounts.ShouldContainKeyAndValue("int", 1);
                wordCounts.ShouldContainKeyAndValue("value", 1);
            }
        }

        public class When_names_are_not_split
        {
            [Fact]
            public void Names_should_not_be_split()
            {
                var counter = new CSharpWordCounter { SplitNames = false };

                var wordCounts = counter.GetWordCounts("public class TestClass { private int camelCaseVariable; }");

                wordCounts.Count.ShouldBe(2);
                wordCounts.ShouldContainKeyAndValue("testclass", 1);
                wordCounts.ShouldContainKeyAndValue("camelcasevariable", 1);
            }
        }
    }
}
