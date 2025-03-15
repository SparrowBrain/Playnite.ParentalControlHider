using ReleaseTools.InstallerManifestYaml;
using Xunit;

namespace ReleaseTools.IntegrationTests.InstallerManifestYaml
{
    public class PlayniteSdkVersionParserTests
    {
        private readonly string _projectFile = $@"InstallerManifestYaml\TestData\{Program.AddonName}.csproj";

        [Fact]
        public void GetVersion_ReturnsVersionFromProjectFile()
        {
            // Arrange
            var sut = new PlayniteSdkVersionParser(_projectFile);

            // Act
            var result = sut.GetVersion();

            // Assert
            Assert.Equal("70.50.60", result);
        }
    }
}