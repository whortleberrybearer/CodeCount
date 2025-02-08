using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CodeCount;

class Program
{
    static void Main(string[] args)
    {
        var config = ReadAndValidateConfig("config.json");

        if (config == null)
        {
            return;
        }

        var fileSearcher = new FileSearcher() 
        { 
            ExcludeFilter = config.ExcludeFilter 
        };

        var wordCounterSelector = new WordCounterSelector();
        RegisterWordCounters(wordCounterSelector, config.WordCounters);

        var aggregator = new WordCountAggregator(fileSearcher, wordCounterSelector)
        {
            FileExtensions = config.ValidFileExtensions,
            MaxResults = config.MaxResults,
            ExcludedWords = config.ExcludeWords?.Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase))
        };

        var wordCounts = aggregator.AggregateWordCounts(config.SourceDirectoryPath);

        WriteOutputFile(wordCounts, config.OutputFilePath);
    }

    private static Config? ReadAndValidateConfig(string configFilePath)
    {
        var configJson = File.ReadAllText(configFilePath);
        var config = JsonConvert.DeserializeObject<Config>(configJson);

        if (string.IsNullOrEmpty(config?.SourceDirectoryPath) || string.IsNullOrEmpty(config?.OutputFilePath))
        {
            Console.WriteLine("Source directory path and output file path must be specified in the config file.");

            return null;
        }

        return config;
    }

    private static void WriteOutputFile(IEnumerable<WordCountResult> wordCounts, string outputFilePath)
    {
        var json = JsonConvert.SerializeObject(wordCounts, Formatting.Indented);
        File.WriteAllText(outputFilePath, json);

        Console.WriteLine($"Word counts have been written to {outputFilePath}");
    }

    private static void RegisterWordCounters(IWordCounterSelector selector, List<WordCounterConfig>? wordCounterConfigs)
    {
        if (wordCounterConfigs == null) return;

        foreach (var wordCounterConfig in wordCounterConfigs)
        {
            var type = Type.GetType(wordCounterConfig.Type);
            if (type == null) throw new InvalidOperationException($"Type {wordCounterConfig.Type} not found.");

            var wordCounter = (IWordCounter)Activator.CreateInstance(type)!;

            if (wordCounterConfig.Properties != null)
            {
                foreach (var property in wordCounterConfig.Properties)
                {
                    var propInfo = type.GetProperty(property.Key);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        propInfo.SetValue(wordCounter, Convert.ChangeType(property.Value, propInfo.PropertyType));
                    }
                }
            }

            selector.RegisterWordCounter(wordCounterConfig.Filters, wordCounter);
        }
    }
}