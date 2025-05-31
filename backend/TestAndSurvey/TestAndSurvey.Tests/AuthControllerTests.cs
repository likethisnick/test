using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TestAndSurvey.Contracts;
using TestAndSurvey.Controllers;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Services;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TestAndSurvey.Tests.Controllers
{
    public class AuthControllerTests
    {
        private static Mock<UserManager<SurvefyUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<SurvefyUser>>();
            return new Mock<UserManager<SurvefyUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private IConfiguration TestConfig()
        {
            var config = new Dictionary<string, string>
            {
                { "Jwt:Key", "my_super_secure_test_key_1234567890!" }
            };
            return new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }

        private IJwtTokenService MockTokenService(string expectedToken = "mock-token")
        {
            var mock = new Mock<IJwtTokenService>();
            mock.Setup(x => x.GenerateJwtToken(It.IsAny<SurvefyUser>())).Returns(expectedToken);
            return mock.Object;
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsUnauthorized()
        {
            var userManager = MockUserManager();
            userManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((SurvefyUser)null);

            var controller = new AuthController(userManager.Object, TestConfig(), MockTokenService());

            var result = await controller.Login(new LoginDto { Email = "test@mail.com", Password = "123" });

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            var user = new SurvefyUser { Id = "1", Email = "test@mail.com" };
            var userManager = MockUserManager();
            userManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(user);
            userManager.Setup(x => x.CheckPasswordAsync(user, "wrong")).ReturnsAsync(false);

            var controller = new AuthController(userManager.Object, TestConfig(), MockTokenService());

            var result = await controller.Login(new LoginDto { Email = "test@mail.com", Password = "wrong" });

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            var user = new SurvefyUser { Id = "1", Email = "test@mail.com" };
            var userManager = MockUserManager();
            userManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(user);
            userManager.Setup(x => x.CheckPasswordAsync(user, "123")).ReturnsAsync(true);

            var controller = new AuthController(userManager.Object, TestConfig(), MockTokenService("token-123"));

            var result = await controller.Login(new LoginDto { Email = "test@mail.com", Password = "123" });

            var ok = Assert.IsType<OkObjectResult>(result);
            var json = JObject.FromObject(ok.Value);

            Assert.Equal("token-123", json["token"]?.ToString());
            Assert.Equal("1", json["userId"]?.ToString());
        }

        [Fact]
        public async Task Register_FailedCreation_ReturnsBadRequest()
        {
            var userManager = MockUserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<SurvefyUser>(), "123"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email taken" }));

            var controller = new AuthController(userManager.Object, TestConfig(), MockTokenService());

            var result = await controller.Register(new RegisterDto { Email = "test@mail.com", Password = "123" });

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var json = JObject.FromObject(bad.Value);
            var errors = json["errors"]?.ToObject<List<string>>();
            Assert.Contains("Email taken", errors);
        }

        [Fact]
        public async Task Register_Success_ReturnsOkWithToken()
        {
            var userManager = MockUserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<SurvefyUser>(), "123"))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AuthController(userManager.Object, TestConfig(), MockTokenService("token-xyz"));

            var result = await controller.Register(new RegisterDto { Email = "test@mail.com", Password = "123" });
            
            var ok = Assert.IsType<OkObjectResult>(result);
            var json = JObject.FromObject(ok.Value);
            Assert.Equal("token-xyz", json["token"]?.ToString());
        }
    }
}