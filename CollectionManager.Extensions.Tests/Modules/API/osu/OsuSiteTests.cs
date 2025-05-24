namespace CollectionManager.Extensions.Tests.Modules.API.osu;

using CollectionManager.Extensions.Modules.API.osu;
using FluentAssertions;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

public sealed class OsuSiteTests : IDisposable
{
    private readonly OsuSite _osuSite;
    private readonly OsuSite.LogUsernameGeneration _logger;

    public OsuSiteTests()
    {
        _osuSite = new OsuSite();
        _logger = Substitute.For<OsuSite.LogUsernameGeneration>();
    }

    [Fact(SkipExceptions = [typeof(WebException)])]
    public async Task GivenValidParametersWhenInvokedThenReturnsUsernamesAsync()
    {
        List<string> result = await _osuSite.GetUsernamesAsync(0, 50, _logger, TestContext.Current.CancellationToken);

        _ = result.Count.Should().Be(OsuSite.UsersPerPage);
    }

    [Fact]
    public async Task GivenInvalidStartRankWhenInvokedThenThrowsArgumentExceptionAsync()
    {
        Func<Task> action = () => _osuSite.GetUsernamesAsync(-1, 50, _logger, TestContext.Current.CancellationToken);

        _ = await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GivenInvalidEndRankWhenInvokedThenThrowsArgumentExceptionAsync()
    {
        Func<Task> action = () => _osuSite.GetUsernamesAsync(0, 10001, _logger, TestContext.Current.CancellationToken);

        _ = await action.Should().ThrowAsync<ArgumentException>();
    }

    public void Dispose() => _osuSite?.Dispose();
}