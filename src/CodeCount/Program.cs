using Newtonsoft.Json;

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

        var fileSearcher = new FileSearcher();
        var wordCounter = new WordCounter();
        var aggregator = new WordCountAggregator(fileSearcher, wordCounter);

        var wordCounts = aggregator.AggregateWordCounts(config.SourceDirectoryPath);

        WriteOutputFile(wordCounts, config.OutputFilePath);
    }

    private static Config ReadAndValidateConfig(string configFilePath)
    {
        var configJson = File.ReadAllText(configFilePath);
        var config = JsonConvert.DeserializeObject<Config>(configJson);

        if (string.IsNullOrEmpty(config.SourceDirectoryPath) || string.IsNullOrEmpty(config.OutputFilePath))
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
}