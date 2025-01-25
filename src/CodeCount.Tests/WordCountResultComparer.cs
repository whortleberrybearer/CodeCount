public class WordCountResultComparer : IEqualityComparer<WordCountResult>
{
    public bool Equals(WordCountResult x, WordCountResult y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Word == y.Word && x.Count == y.Count;
    }

    public int GetHashCode(WordCountResult obj)
    {
        if (obj == null)
        {
            return 0;
        }

        int hashWord = obj.Word == null ? 0 : obj.Word.GetHashCode();
        int hashCount = obj.Count.GetHashCode();

        return hashWord ^ hashCount;
    }
}