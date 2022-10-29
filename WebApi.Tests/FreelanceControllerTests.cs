using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.ApiClient;
using WebApi.Controllers;
using WebApi.DAL;
using WebApi.Models;
using Xunit.Abstractions;

namespace WebApi.Tests;

public class FreelanceControllerTests
{
    private readonly FreelanceController _sut;
    private readonly ILogger<FreelanceController> _logger = Substitute.For<ILogger<FreelanceController>>();
    private readonly IHttpContextAccessor _contextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IFLDbContext _dbContext = Substitute.For<IFLDbContext>();
    private readonly IFreelancerClient _flClient = Substitute.For<IFreelancerClient>();
    private readonly IFixture _fixture = new Fixture();
    private readonly ITestOutputHelper output;

    public FreelanceControllerTests(ITestOutputHelper _output)
    {
        _sut = new FreelanceController(_logger, _contextAccessor, _dbContext, _flClient);
        output = _output;
    }
    [Fact]
    public void GetServiceConfigState_ShouldReturnAuthUrl_WhenTokenForCurrentUserIsNull()
    {
        //Arrange
        var userId = _fixture.Create<string>();
        //Extension methods cannot be mocked... FindFirstValue from claimsprincipal is extension method
        _contextAccessor.HttpContext!.User.Returns(GetClaimsWithId(userId));
        var apiToken = _fixture.Build<FLApiToken>().With(t => t.UserID, userId).Create();
        _dbContext.FLApiTokens.Where(t => t.UserID == userId).FirstOrDefaultAsync().Returns(Task.FromResult<FLApiToken>(apiToken));
        //Act

        //Assert

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