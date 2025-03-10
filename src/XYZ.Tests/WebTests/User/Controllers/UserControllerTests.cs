using Microsoft.AspNetCore.Mvc;

using Moq;

using Newtonsoft.Json;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Features.User.Controllers;

namespace XYZ.Tests.WebTests.User.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserLogic> _userLogicMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userLogicMock = new Mock<IUserLogic>();
            _controller = new UserController(_userLogicMock.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtAction_WithUserId()
        {
            // Arrange
            var fakeUserId = 123;
            _userLogicMock
                .Setup(x => x.CreateUser())
                .ReturnsAsync(fakeUserId); 

            // Act
            var result = await _controller.CreateUser();

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UserController.CreateUser), createdResult.ActionName);
            var json1 = JsonConvert.SerializeObject(new { id = fakeUserId });
            var json2 = JsonConvert.SerializeObject(createdResult.Value);
            Assert.Equal(json1, json2);
            _userLogicMock.Verify(x => x.CreateUser(), Times.Once);
        }
    }
}
