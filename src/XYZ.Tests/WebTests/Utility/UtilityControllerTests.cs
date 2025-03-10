using Microsoft.AspNetCore.Mvc;

using Moq;

using Newtonsoft.Json;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Utility.Controllers;

namespace XYZ.Tests.WebTests.Utility.Controllers
{
    public class UtilityControllerTests
    {
        private readonly Mock<ISmokeTestLogic> _smokeTestLogicMock;
        private readonly UtilityController _controller;

        public UtilityControllerTests()
        {
            _smokeTestLogicMock = new Mock<ISmokeTestLogic>();
            _controller = new UtilityController(_smokeTestLogicMock.Object);
        }

        [Fact]
        public void Ping_ReturnsOk()
        {
            // Act
            var result = _controller.Ping();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SmokeTest_ReturnsOk_WithSystemFunctionalStatus()
        {
            // Arrange
            var expectedResult = true;
            _smokeTestLogicMock
                .Setup(x => x.IsSystemFunctional())
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SmokeTest();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var json1 = JsonConvert.SerializeObject(new { IsSystemFunctional = true });
            var json2 = JsonConvert.SerializeObject(okObjectResult.Value);
            Assert.Equal(json1, json2);

            _smokeTestLogicMock.Verify(x => x.IsSystemFunctional(), Times.Once);
        }
    }
}
