namespace CodeCount;

using Cocona;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        CoconaApp.Run<Program>(args);
    }

    public void Run(
        [Option("source-directory-path", ['s'], Description = "The directory scan")] string? sourceDirectoryPath,
        [Option("output-file-path", ['o'], Description = "The file to write the output to")] string? outputFilePath,
        [Option("config-file-path", ['c'], Description = "The path to the config file")] string? configFilePath)
    {
        var config = ReadAndValidateConfig(configFilePath ?? "config.json");

        if (config == null)
        {
            return;
        }

        var fileSearcher = new FileSearcher()
        {
            ExcludeFilter = config.ExcludeFilter
        };

        var wordCounterSelector = CreateWordCounterSelector(config);

        var aggregator = new WordCountAggregator(fileSearcher, wordCounterSelector)
        {
            MaxResults = config.MaxResults,
            ExcludedWords = config.ExcludeWords?.Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase))
        };

        var wordCounts = aggregator.AggregateWordCounts(sourceDirectoryPath ?? ".");

        WriteOutputFile(wordCounts, outputFilePath ?? "output.json");
    }

    private static WordCounterSelector CreateWordCounterSelector(Config config)
    {
        var wordCounterSelector = new WordCounterSelector();

        if (config.WordCounters is not null)
        {
            foreach (var wordCounterConfig in config.WordCounters)
            {
                wordCounterSelector.RegisterWordCounter(
                    wordCounterConfig.Filters, 
                    WordCounterFactory.CreateWordCounter(wordCounterConfig));
            }
        }

        return wordCounterSelector;
    }

    private static Config? ReadAndValidateConfig(string configFilePath)
    {
        var configJson = File.ReadAllText(configFilePath);
        
        // Currently no validation is required.
        return JsonConvert.DeserializeObject<Config>(configJson);
    }

    private static void WriteOutputFile(IEnumerable<WordCountResult> wordCounts, string outputFilePath)
    {
        var json = JsonConvert.SerializeObject(wordCounts, Formatting.Indented);
        File.WriteAllText(outputFilePath, json);

        Console.WriteLine($"Word counts have been written to {outputFilePath}");
    }
}