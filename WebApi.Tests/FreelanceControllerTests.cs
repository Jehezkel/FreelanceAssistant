using System.Security.Claims;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
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
        _contextAccessor.HttpContext.User.Returns(GetClaimsWithId(userId));
        var apiToken = _fixture.Build<FLApiToken>().With(t => t.UserID, userId).Create();
        _flApiTokenRepository.GetAccessToken(userId).Returns<Task>(i => null);
        //Act
        var result = await _sut.GetServiceConfigState();

        var okObjectResult = (OkObjectResult)result;
        //Assert
        // Assert.Equal(new { authUrl = _flClient.getAuthorizationUrl() }, result.AS);
        okObjectResult.Value.Should().BeEquivalentTo(new { authUrl = _flClient.getAuthorizationUrl() });
        okObjectResult.StatusCode.Should().Be(200);
    }

    private void Ok()
    {
        throw new NotImplementedException();
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