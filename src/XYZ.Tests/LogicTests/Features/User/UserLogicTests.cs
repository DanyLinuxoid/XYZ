using Moq;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.DataAccess.Tables.USER_TBL.Queries;
using XYZ.Logic.Features.User;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Tests.LogicTests.Features.User
{
    public class UserLogicTests
    {
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly UserLogic _userLogic;

        public UserLogicTests()
        {
            _databaseLogicMock = new Mock<IDatabaseLogic>();
            _userLogic = new UserLogic(_databaseLogicMock.Object);
        }

        [Fact]
        public async Task GetUserInfoShort_ReturnsUserInfoShort_WhenUserExists()
        {
            // Arrange
            var userId = 12345L;
            var user = new USER
            {
                ID = userId,
                DB_RECORD_CREATION_TIME = DateTime.Now
            };
            var expectedDto = new UserDto
            {
                Id = userId,
                CreationTime = user.DB_RECORD_CREATION_TIME
            };

            _databaseLogicMock.Setup(x => x.QueryAsync(It.IsAny<UserGetByIdQuery>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userLogic.GetUserInfo(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.CreationTime, result.CreationTime);
            _databaseLogicMock.Verify(x => x.QueryAsync(It.IsAny<UserGetByIdQuery>()), Times.Once);
        }

        [Fact]
        public async Task GetUserInfoShort_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 12345L;
            _databaseLogicMock.Setup(x => x.QueryAsync(It.IsAny<UserGetByIdQuery>()))
                .ReturnsAsync((USER?)null);

            // Act
            var result = await _userLogic.GetUserInfo(userId);

            // Assert
            Assert.Null(result);
            _databaseLogicMock.Verify(x => x.QueryAsync(It.IsAny<UserGetByIdQuery>()), Times.Once);
        }
    }
}
