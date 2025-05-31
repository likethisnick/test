using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MXDevelop.Controllers;
using TestAndSurvey.Contracts;
using TestAndSurvey.Controllers;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;
using Xunit;

namespace TestAndSurvey.Tests.Controllers
{
    public class TemplateSurveysControllerTests
    {
        private static ClaimsPrincipal CreateUser(string userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuth"));
        }

        private static TemplateSurveysController CreateController(SurvefyDbContext context, ClaimsPrincipal user)
        {
            var controller = new TemplateSurveysController(context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        [Fact]
        public async Task GetSurveys_ReturnsSurveysForGivenUser()
        {
            var userId = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var dbContext = new SurvefyDbContext(options);

            dbContext.TemplateSurvey.Add(new TemplateSurvey
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = userId,
                CreatedOn = DateTime.UtcNow.AddMinutes(-1),
                Name = "Survey A",
                Description = "Description A"
            });

            dbContext.TemplateSurvey.Add(new TemplateSurvey
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = userId,
                CreatedOn = DateTime.UtcNow,
                Name = "Survey B",
                Description = "Description B"
            });

            await dbContext.SaveChangesAsync();

            var controller = CreateController(dbContext, CreateUser(userId));
            
            var result = await controller.GetSurveys(new GetTemplateSurveysRequest { CreatedByUserId = userId }, CancellationToken.None);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetTemplateSurveysResponse>(okResult.Value);
            Assert.Equal(2, response.Surveys.Count);
            Assert.Equal("Survey B", response.Surveys[0].Name); // latest first
        }

        [Fact]
        public async Task GetSurveys_ReturnsUnauthorized_WhenNoUser()
        {
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var dbContext = new SurvefyDbContext(options);
            var controller = CreateController(dbContext, new ClaimsPrincipal());
            
            var result = await controller.GetSurveys(new GetTemplateSurveysRequest { CreatedByUserId = "abc" }, CancellationToken.None);
            
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetSurveys_ReturnsBadRequest_WhenMissingCreatedByUserId()
        {
            var userId = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var dbContext = new SurvefyDbContext(options);
            var controller = CreateController(dbContext, CreateUser(userId));
            
            var result = await controller.GetSurveys(new GetTemplateSurveysRequest { CreatedByUserId = null }, CancellationToken.None);
            
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("CreatedByUserId is required", badResult.Value);
        }

        [Fact]
        public async Task GetAllSurveys_ReturnsAllSurveys()
        {
            var userId = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<SurvefyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var dbContext = new SurvefyDbContext(options);

            dbContext.TemplateSurvey.AddRange(
                new TemplateSurvey
                {
                    Id = Guid.NewGuid(),
                    CreatedByUserId = userId,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Name = "Survey X",
                    Description = "Desc X"
                },
                new TemplateSurvey
                {
                    Id = Guid.NewGuid(),
                    CreatedByUserId = "someoneelse",
                    CreatedOn = DateTime.UtcNow,
                    Name = "Survey Y",
                    Description = "Desc Y"
                });

            await dbContext.SaveChangesAsync();

            var controller = CreateController(dbContext, CreateUser(userId));

            var result = await controller.GetAllSurveys(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetTemplateSurveysResponse>(okResult.Value);
            Assert.Equal(2, response.Surveys.Count);
        }
    }
}