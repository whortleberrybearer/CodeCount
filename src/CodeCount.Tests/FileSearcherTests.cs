public class FileSearcherTests
{
    public class When_getting_files
    {
        [Fact]
        public void Returns_all_files_in_directory_and_subdirectories()
        {
            var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectory);

            var subDirectory = Path.Combine(testDirectory, "subdir");
            Directory.CreateDirectory(subDirectory);

            var file1 = Path.Combine(testDirectory, "file1.txt");
            var file2 = Path.Combine(subDirectory, "file2.txt");

            var sut = new FileSearcher();

            try
            {
                var results = sut.GetAllFiles(testDirectory).ToArray();

                results.Length.ShouldBe(2);
                results.ShouldContain(file1);
                results.ShouldContain(file2);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }
    }
}