using System.Security.Claims;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using WebApi.ApiClient;
using WebApi.Controllers;
using WebApi.DAL;
using WebApi.Models;
using WebApi.Repositories;
using Xunit.Abstractions;

namespace WebApi.Tests;

public class FreelanceControllerTests
{
    private readonly FreelanceController _sut;
    private readonly ILogger<FreelanceController> _logger = Substitute.For<ILogger<FreelanceController>>();
    private readonly IHttpContextAccessor _contextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IFreelancerClient _flClient = Substitute.For<IFreelancerClient>();
    private readonly IFLApiTokenRepository _flApiTokenRepository = Substitute.For<IFLApiTokenRepository>();
    private readonly IFixture _fixture = new Fixture();
    private readonly ITestOutputHelper output;

    public FreelanceControllerTests(ITestOutputHelper _output)
    {
        _sut = new FreelanceController(_logger, _contextAccessor, _flClient, _flApiTokenRepository);
        output = _output;
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    [Fact]
    public async void GetServiceConfigState_ShouldReturnAuthUrl_WhenTokenForCurrentUserIsNull()
    {
        //Arrange
        var userId = _fixture.Create<string>();
        _contextAccessor.HttpContext!.User.Returns(GetClaimsWithId(userId));
        var apiToken = _fixture.Build<FLApiToken>().With(t => t.UserID, userId).Create();
        _flApiTokenRepository.GetAccessToken(userId).ReturnsNull();
        //Act
        var result = await _sut.GetServiceConfigState();

        var okObjectResult = (OkObjectResult)result;
        //Assert
        okObjectResult.Value.Should().BeEquivalentTo(new { authUrl = _flClient.getAuthorizationUrl() });
        okObjectResult.StatusCode.Should().Be(200);
    }
    [Fact]
    public async void GetServiceConfigState_ShouldReturnOk_WhenTokenForCurrentUserIsNotNull()
    {
        //Arrange
        var userId = _fixture.Create<string>();
        _contextAccessor.HttpContext!.User.Returns(GetClaimsWithId(userId));
        var apiToken = _fixture.Build<FLApiToken>().With(t => t.UserID, userId).Create();
        _flApiTokenRepository.GetAccessToken(userId).Returns(apiToken.AccessToken);
        //Act
        var result = await _sut.GetServiceConfigState();
        var okResult = (OkResult)result;
        //Assert
        result.Should().BeOfType<OkResult>();
        okResult.StatusCode.Should().Be(200);
    }

    private ClaimsPrincipal GetClaimsWithId(string UserId)
    {
        List<Claim> claimList = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, UserId)
        };
        ClaimsIdentity identity = new ClaimsIdentity(claimList);
        return new ClaimsPrincipal(identity);
    }
}
