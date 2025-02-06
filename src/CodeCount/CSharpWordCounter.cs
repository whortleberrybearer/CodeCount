using System.Text.RegularExpressions;

public class CSharpWordCounter : WordCounter
{
    public CSharpWordCounter() 
        : base(new Regex(@"[^a-zA-Z_]+"))
    {
    }

    protected override IEnumerable<string> SplitText(string text)
    {
        // Use the base class's SplitText method and add additional logic if needed
        return base.SplitText(text);
    }
}
