# CodeCount

A simple console program that counts the number of words in files.

## Background

While watching a conference session, the speaker (unfortunately I can't remember who it was) said if you look at the most common words in code, it should tell you about the program / what it does. If you can't, the program may not be well written.

At least, that's what I think the gist was, it was a long time ago.

## How to Run

The program will run without additional configuration, however, the following options can be set:

| Option                      | Description                                                         |
|-----------------------------|---------------------------------------------------------------------|
| --source-directory-path, -s | The directory to scan. Current directory if not specified.          |
| --output-file-path, -o      | The file to write the output to. CodeCount.json if not specified.   |
| --config-file-path, -c      | The path to the config file. Uses [default configuration](#default-configuration) if not specified. |    

### Examples

#### Basic

Scan the current directory and write output to `CodeCount.json` in the current directory. All files will be processed using the [default configuration](#default-configuration).

```cmd
CodeCount.exe
```

#### Full

Scan the `/Test` directory and write output to `/Result/Output.json`. All files will be processed using the configuration defined in `Test.config`.

```cmd
CodeCount.exe -s "/Test" -o "/Result/Output.json" -c "Test.config"
```

## Configuration

The following properties can be set within the config file:

- excludeFilter - A list of glob filters for excluding directories and files.
- excludeWords - A list of words to exclude. Supports regular expressions.
- maxResults - Limits the number of results based on the count of each word.
- wordCounters - The word counters to process files with.

### Default Configuration

The default configuration will process all *.cs files with the [CSharpWordCounter](#csharpcounter) and everything else with the standard [WordCounter](#wordcounter).

```json
{
    "wordCounters": [
        {
            "type": "CSharpWordCounter",
            "filters": [ "**/*.cs" ]
        },
        {
            "type": "WordCounter",
            "filters": [ "**/*" ]
        }
    ]
}
```

### Example

Will ignore anything in the folders specified in `excludeFilter`. .cs files will be processed by the CSharp word counter and .md files by the standard word counter. All other files will be ignored.

Words present in `excludeWords` will be removed from the results, and only the top `maxResults` will be returned based on the number of instances of the word.

```json
{
    "wordCounters": [
        {
            "type": "CSharpWordCounter",
            "filters": [ "**/*.cs" ],
            "properties": {
                "excludeKeywords": true,
                "splitNames": true
            }
        },
        {
            "type": "WordCounter",
            "filters": [ "**/*.md" ]
        }
    ],
    "excludeFilter": [ "**/*.Tests*", "**/bin", "**/obj", "**/.vs" ],
    "excludeWords": [ "system", "global", "new", "public", "var", "using" ],
    "maxResults": 10
}
```

## Supported Counters

### WordCounter

A simplistic counter that splits words on spaces and punctuation.

#### Options

- SplitExpression (default "[^a-zA-Z0-9]+") - The expression to split words with.

### CSharpCounter

A specialized counter for C# files. Excludes numbers when splitting words and by default will exclude language keywords and split any camel or pascal cased named items.

#### Options

- SplitExpression (default "[^a-zA-Z]+") - The expression to split words with.
- ExcludeKeywords (default true) - If language keywords should be excluded from the count.
- SplitNames (default true) - If pascal or camel case named items should be split.
