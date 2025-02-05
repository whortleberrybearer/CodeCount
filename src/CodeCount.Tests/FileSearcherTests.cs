public class FileSearcherTests
{
    public class When_getting_files
    {
        [Fact]
        public void Returns_all_files_in_directory_and_subdirectories()
        {
            var testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectoryPath);

            var subDirectoryPath = Path.Combine(testDirectoryPath, "subdir");
            Directory.CreateDirectory(subDirectoryPath);

            var file1Path = Path.Combine(testDirectoryPath, "file1.txt");
            var file2Path = Path.Combine(subDirectoryPath, "file2.txt");

            File.WriteAllText(file1Path, "Test content");
            File.WriteAllText(file2Path, "Test content");

            var sut = new FileSearcher();

            try
            {
                var results = sut.GetAllFiles(testDirectoryPath).ToArray();

                results.Length.ShouldBe(2);
                results.ShouldContain(file => file.FullName == file1Path);
                results.ShouldContain(file => file.FullName == file2Path);
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }
    }

    [Fact]
    public void Should_apply_filter_when_searching_files()
    {
        var directoryPath = Path.GetTempPath();
        var fileSearcher = new FileSearcher { Filter = "*.txt" };

        var tempFile1 = Path.Combine(directoryPath, "test1.txt");
        var tempFile2 = Path.Combine(directoryPath, "test2.doc");

        try
        {
            File.WriteAllText(tempFile1, "test");
            File.WriteAllText(tempFile2, "test");

            var files = fileSearcher.GetAllFiles(directoryPath).ToList();

            //files.Count.ShouldBe(1);
            files[0].FullName.ShouldBe(tempFile1);
        }
        finally
        {
            File.Delete(tempFile1);
            File.Delete(tempFile2);
        }
    }

    [Fact]
    public void Should_return_all_files_when_no_filter_is_set()
    {
        var directoryPath = Path.GetTempPath();
        var fileSearcher = new FileSearcher();

        var tempFile1 = Path.Combine(directoryPath, "test1.txt");
        var tempFile2 = Path.Combine(directoryPath, "test2.doc");

        try
        {
            File.WriteAllText(tempFile1, "test");
            File.WriteAllText(tempFile2, "test");

            var files = fileSearcher.GetAllFiles(directoryPath).ToList();

            //files.Count.ShouldBe(2);
            files.ShouldContain(f => f.FullName == tempFile1);
            files.ShouldContain(f => f.FullName == tempFile2);
        }
        finally
        {
            File.Delete(tempFile1);
            File.Delete(tempFile2);
        }
    }
}