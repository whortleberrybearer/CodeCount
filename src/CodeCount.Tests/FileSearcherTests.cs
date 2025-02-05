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

                results.ShouldContain(file => file.FullName == file1Path);
                results.ShouldContain(file => file.FullName == file2Path);
            }
            finally
            {
                Directory.Delete(testDirectoryPath, true);
            }
        }

        public class And_filter_applied
        {
            [Fact]
            public void Should_not_return_files_matching_filter()
            {
                var testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(testDirectoryPath);

                var subDirectoryPath = Path.Combine(testDirectoryPath, "subdir");
                Directory.CreateDirectory(subDirectoryPath);

                var file1Path = Path.Combine(testDirectoryPath, "file1.txt");
                var file2Path = Path.Combine(testDirectoryPath, "file2.doc");
                var file3Path = Path.Combine(testDirectoryPath, "file3.exe");
                var file4Path = Path.Combine(subDirectoryPath, "test4.doc");

                File.WriteAllText(file1Path, "Test content");
                File.WriteAllText(file2Path, "Test content");
                File.WriteAllText(file3Path, "Test content");
                File.WriteAllText(file4Path, "Test content");

                var sut = new FileSearcher() 
                { 
                    ExcludeFilter = "**/*.doc;*.exe" 
                };

                try
                {
                    var results = sut.GetAllFiles(testDirectoryPath).ToArray();

                    results.ShouldContain(file => file.FullName == file1Path);
                    results.ShouldNotContain(file => file.FullName == file2Path);
                    results.ShouldNotContain(file => file.FullName == file3Path);
                    results.ShouldNotContain(file => file.FullName == file4Path);
                }
                finally
                {
                    Directory.Delete(testDirectoryPath, true);
                }
            }
        }
    }
}