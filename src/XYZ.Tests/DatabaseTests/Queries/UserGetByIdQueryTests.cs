using Moq;

using Newtonsoft.Json;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.DataAccess.Tables.USER_TBL.Queries;

namespace XYZ.Tests.DataAccess.Tables.USER_TBL.Queries
{
    public class UserGetByIdQueryTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = 42;
            var expectedUser = new USER { ID = userId };

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<USER?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expectedUser);

            var query = new UserGetByIdQuery(userId);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.ID, result.ID);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<USER?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((USER?)null);

            var query = new UserGetByIdQuery(userId);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_CallsQueryWithCorrectParameters()
        {
            // Arrange
            var userId = 77;
            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            var query = new UserGetByIdQuery(userId);

            // Act
            await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            databaseQueryMock.Verify(x =>
                x.QueryFirstOrDefaultAsync<USER?>(It.IsAny<string>(), It.Is<object>(p =>
                    JsonConvert.SerializeObject(p) ==
                    JsonConvert.SerializeObject(new { _id = userId })
                )), Times.Once);
        }
    }
}
