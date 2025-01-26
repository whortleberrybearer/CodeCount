public class WordCountResultComparer : IEqualityComparer<WordCountResult>
{
    public bool Equals(WordCountResult? x, WordCountResult? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.Word == y.Word && x.Count == y.Count;
    }

    public int GetHashCode(WordCountResult obj)
    {
        if (obj is null)
        {
            return 0;
        }

        int hashWord = obj.Word is null ? 0 : obj.Word.GetHashCode();
        int hashCount = obj.Count.GetHashCode();

        return hashWord ^ hashCount;
    }
}