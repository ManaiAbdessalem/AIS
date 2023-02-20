using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Assert = Xunit.Assert;

namespace AIS_UnitTest
{
    public class DownloadServiceTests
    {
        /// <summary>
        /// Class <see cref="EmployeeControllerUnitTests"/> containing unit tests of service  <see cref="DownloadService"/>.
        /// </summary>
        public class EmployeeControllerUnitTests
        {
            public readonly DownloadService _downloadService;

            /// <summary>
            /// Constructor of EmployeeControllerUnitTests.
            /// </summary>
            public EmployeeControllerUnitTests()
            {
                _downloadService = new DownloadService();
            }

            /// <summary>
            /// DownloadFile Ok.
            /// </summary>
            [Theory]
            [InlineData("C:")]
            public async Task DownloadFile_Ok(string filepath)
            {
                // Arrange Act
                await _downloadService.DownloadFilesAsync(filepath);

                // Assert
                Assert.True(true);
            }

            /// <summary>
            /// DownloadFile Folder Not Exists throw Exception.
            /// </summary>
            [Theory]
            [InlineData("CCC:")]
            [ExpectedException(typeof(Exception))]
            public async Task DownloadFile_FolderNotExists_throwException(string filepath)
            {
                // Arrange Act
                var ex = Assert.ThrowsAsync<DirectoryNotFoundException>(() => _downloadService.DownloadFilesAsync(filepath));

                // Assert
                Assert.Equal("Local folder path invalid!", ex.Result.Message);
            }
        }
    }
}