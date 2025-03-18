using AutoFixture.Xunit2;
using ReleaseTools.Package;
using Xunit;

namespace ReleaseTools.UnitTests.Package
{
    public class ExtensionPackageNameGuesserTests
    {
        [Theory]
        [InlineAutoData("1.1.0", "ParentalControlHider_134725de-cfcb-4474-849b-5d9c52babb75_1_1_0.pext")]
        [InlineAutoData("20.30.404", "ParentalControlHider_134725de-cfcb-4474-849b-5d9c52babb75_20_30_404.pext")]
        public void GetName_ProviderNameWithinConvention(
            string version,
            string expected,
            ExtensionPackageNameGuesser sut)
        {
            // Act
            var actual = sut.GetName(version);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}