using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TestAndSurvey.Controllers;
using TestAndSurvey.Contracts;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TestAndSurvey.Tests.Controllers
{
    public class NewSurveyControllerTests
    {
        private static Mock<UserManager<SurvefyUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<SurvefyUser>>();
            return new Mock<UserManager<SurvefyUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private IConfiguration TestConfig()
        {
            var config = new Dictionary<string, string> { { "Jwt:Key", "test-key" } };
            return new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }

        private static ClaimsPrincipal MockClaimsPrincipal(string userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task CreateTemplateSurvey_Unauthorized_WhenUserIdMissing()
        {
            var userManager = MockUserManager();
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            await using var dbContext = new SurvefyDbContext(options);

            var controller = new NewSurveyController(userManager.Object, TestConfig(), dbContext);
            
            var emptyPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = emptyPrincipal
                }
            };

            var request = new CreateTemplateSurveyRequest
            {
                Name = "Unauthorized Test",
                Description = "Should fail"
            };
            
            var result = await controller.CreateTemplateSurvey(request, CancellationToken.None);
            
            Assert.IsType<UnauthorizedResult>(result);
        }


        [Fact]
        public async Task CreateTemplateSurvey_Success_ReturnsOkWithId()
        {
            var userId = Guid.NewGuid().ToString();
            
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var dbContext = new SurvefyDbContext(options);

            var userManager = MockUserManager();

            var controller = new NewSurveyController(userManager.Object, TestConfig(), dbContext);
            
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var request = new CreateTemplateSurveyRequest
            {
                Name = "Test Survey",
                Description = "Test Description"
            };

            var result = await controller.CreateTemplateSurvey(request, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var idProperty = okResult.Value.GetType().GetProperty("Id");
            Assert.NotNull(idProperty);
            var idValue = idProperty.GetValue(okResult.Value);
            Assert.NotNull(idValue);
            Assert.IsType<Guid>(idValue);
            Assert.NotEqual(Guid.Empty, (Guid)idValue);
        }

    }
}
