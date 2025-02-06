# CodeCount

A simple console program that counts the number of words in files.

## Background

While watching an conference session, the speaker (unfortunately I can't remember who it was) said if you look at the most common words in code, is should tell you about the program / what it does.  If you can't, the program may not be well written.

Atleast, thats what I think the gist was, it was a long time ago.

## Configuration

Configuration is set within the `config.json` file.  The following properties can be set:

- sourceDirectoryPath (**Required**) - The path to scan.
- outputFilePath (**Required**) - The path and filename to write the output file to.
- excludeFilter - A list of glob filters for exlcuding directories and files.
- excludeWords - A list of words to exclude.  Supports regular expressions.
- maxResults - Limits the number of results based on the count of each word.
- validFileExtensions - A list of extensions that will be processed.

## Examples

### Basic

Scan the current directory and write to `output.json` in the current directory.

```json
{
    "sourceDirectoryPath": ".",
    "outputFilePath": "output.json"
}
```

### Full

Scan the current directory and write to `output.json` in the current directory.  Will ignore anything in the folders specified in `excludeFilter`, and only process files of the types in `validFileExtensions`.

Words present in `excludeWords` will be removed from the results, and only the top `maxResults` will be returned based on the number of instances of the word.

```json
{
    "sourceDirectoryPath": ".",
    "outputFilePath": "output.json",
    "excludeFilter": [ "**/*.Tests*", "**/bin", "**/obj", "**/.vs" ],
    "validFileExtensions": [".cs", ".md"],
    "excludeWords": [ "system", "global", "new", "public", "var", "using" ],
    "maxResults": 10
}
```
