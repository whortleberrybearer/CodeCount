using System;
using System.Collections.Generic;
using System.IO;

public class FileSearcher
{
    public IEnumerable<string> GetAllFiles(string directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            throw new ArgumentException("Directory path cannot be null or empty", nameof(directoryPath));
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
        }

        return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);
    }
}