namespace CodeCount;

public static class WordCounterFactory
{
    public static IWordCounter CreateWordCounter(WordCounterConfig config)
    {
        var type = Type.GetType($"CodeCount.{config.Type}");

        if (type is null) 
        {
            throw new InvalidOperationException($"Type {config.Type} not found.");
        }

        var wordCounter = (IWordCounter)Activator.CreateInstance(type)!;

        if (config.Properties is not null)
        {
            foreach (var property in config.Properties)
            {
                var propInfo = type.GetProperty(property.Key);

                if (propInfo is not null && propInfo.CanWrite)
                {
                    propInfo.SetValue(wordCounter, Convert.ChangeType(property.Value, propInfo.PropertyType));
                }
            }
        }

        return wordCounter;
    }
}
