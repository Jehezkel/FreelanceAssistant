using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.ApiClient;
using WebApi.Controllers;
using WebApi.DAL;
using WebApi.Models;

namespace WebApi.Tests;

public class FreelanceControllerTests
{
    private readonly FreelanceController _sut;
    private readonly ILogger<FreelanceController> _logger = Substitute.For<ILogger<FreelanceController>>();
    private readonly IHttpContextAccessor _contextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IFLDbContext _dbContext = Substitute.For<IFLDbContext>();
    private readonly IFreelancerClient _flClient = Substitute.For<IFreelancerClient>();
    private readonly IFixture _fixture = new Fixture();

    public FreelanceControllerTests()
    {
        _sut = new FreelanceController(_logger, _contextAccessor, _dbContext, _flClient);
    }
    [Fact]
    public void GetServiceConfigState_ShouldReturnAuthUrl_WhenTokenForCurrentUserIsNull()
    {
        //Arrange
        var userId = _fixture.Create<string>();
        _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).Returns(userId);
        // _dbContext.FLApiTokens.When
        //Act

        //Assert

    }
}