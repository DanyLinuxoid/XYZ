using Moq;

using Newtonsoft.Json;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;

namespace XYZ.Tests.DataAccess.Tables.ORDER_TBL.Queries
{
    public class OrderByOrderNumberAndUserIdGetQueryTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var userId = 1;
            var orderNumber = 1001;
            var expectedOrder = new ORDER { ORDER_NUMBER = orderNumber, USER_ID = userId };

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<ORDER?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expectedOrder);

            var query = new OrderByOrderNumberAndUserIdGetQuery(userId, orderNumber);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.ORDER_NUMBER, result.ORDER_NUMBER);
            Assert.Equal(expectedOrder.USER_ID, result.USER_ID);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var userId = 2;
            var orderNumber = 2002;

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<ORDER?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((ORDER?)null);

            var query = new OrderByOrderNumberAndUserIdGetQuery(userId, orderNumber);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_CallsQueryWithCorrectParameters()
        {
            // Arrange
            var userId = 3;
            var orderNumber = 3003;
            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            var query = new OrderByOrderNumberAndUserIdGetQuery(userId, orderNumber);

            // Act
            await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            databaseQueryMock.Verify(x =>
                x.QueryFirstOrDefaultAsync<ORDER?>(It.IsAny<string>(), It.Is<object>(p =>
                    JsonConvert.SerializeObject(p) ==
                    JsonConvert.SerializeObject(new { _userId = userId, _orderNumber = orderNumber })
                )), Times.Once);
        }
    }
}
