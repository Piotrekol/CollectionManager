namespace CollectionManager.Core.Tests.Extensions;

using CollectionManager.Core.Extensions;
using FluentAssertions;
using Xunit;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("validfilename.txt", "validfilename.txt")]
    [InlineData("inva|id:fi*le?name.txt", "invaidfilename.txt")]
    [InlineData("test<file>name.txt", "testfilename.txt")]
    [InlineData("file\"name.txt", "filename.txt")]
    [InlineData("file/name\\name.txt", "filenamename.txt")]
    [InlineData("file|name?.txt", "filename.txt")]
    public void StripInvalidFileNameCharactersRemovesInvalidChars(string input, string expected)
    {
        string result = input.StripInvalidFileNameCharacters();

        _ = expected.Should().Be(result);
    }

    [Theory]
    [InlineData("inva|id:fi*le?name.txt", "_", "inva_id_fi_le_name.txt")]
    [InlineData("file/name\\name.txt", "-", "file-name-name.txt")]
    [InlineData("file|name?.txt", "X", "fileXnameX.txt")]
    public void StripInvalidFileNameCharactersReplacesWithCustomString(string input, string replacement, string expected)
    {
        string result = input.StripInvalidFileNameCharacters(replacement);

        _ = expected.Should().Be(result);
    }

    [Fact]
    public void StripInvalidFileNameCharactersNullInputThrowsArgumentNullException()
        => Assert.Throws<System.ArgumentNullException>(() => ((string)null).StripInvalidFileNameCharacters());

    [Fact]
    public static void StripInvalidFileNameCharactersEmptyInputThrowsArgumentNullException()
        => Assert.Throws<System.ArgumentNullException>(() => string.Empty.StripInvalidFileNameCharacters());

}