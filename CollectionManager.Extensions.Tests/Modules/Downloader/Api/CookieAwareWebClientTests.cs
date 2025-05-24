namespace CollectionManager.Extensions.Tests.Modules.Downloader.Api;

using CollectionManager.Extensions.Modules.Downloader.Api;
using Xunit;

public class CookieAwareWebClientTests
{
    [Theory]
    [InlineData("osu_session=#####; expires=Tue, 03-Jun-2025 04:23:13 GMT; Max-Age=2592000; path=/; domain=.ppy.sh; secure; HttpOnly; SameSite=lax")]
    [InlineData("osu_session=YYYYYYYYY")]
    public void SetCookiesRunsSuccessfully(string cookie)
    {
        using CookieAwareWebClient client = new();
        client.SetCookies(cookie, [], ".some.domain");
    }
}