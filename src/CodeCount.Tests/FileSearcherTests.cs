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
}