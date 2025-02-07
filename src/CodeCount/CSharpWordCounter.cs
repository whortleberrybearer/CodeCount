using System.Text.RegularExpressions;

public class CSharpWordCounter : WordCounter
{
    public CSharpWordCounter() 
        : base()
    {
    }

    public bool ExcludeKeywords { get; set; } = true;

    public bool SplitNames { get; set; } = true;

    protected override IEnumerable<string> SplitText(string text)
    {
        var words = base.SplitText(text);

        if (ExcludeKeywords)
        {
            // Taken from https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/
            var keywords = new HashSet<string>
            {
                "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
                "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
                "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
                "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
                "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
                "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
                "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "allows",
                "alias", "and", "ascending", "args", "async", "await", "by", "descending", "dynamic", "equals", "field",
                "file", "from", "get", "global", "group", "init", "into", "join", "let", "managed", "nameof", "nint", "not",
                "notnull", "nuint", "on", "or", "orderby", "partial", "record", "remove", "required", "scoped", "select", 
                "set", "unmanaged", "value", "var", "when", "where", "with", "yield"
            };
            
            words = words.Where(word => !keywords.Contains(word));
        }

        if (SplitNames)
        {
            words = words.SelectMany(w => SplitCamelAndPascalCase(w));
        }

        return words;
    }

    private IEnumerable<string> SplitCamelAndPascalCase(string word)
    {
        // This magic looking code should split variable names from camel or pascal case to separate words.
        return base.SplitText(Regex.Replace(word, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)", " $1", RegexOptions.Compiled));
    }
}
