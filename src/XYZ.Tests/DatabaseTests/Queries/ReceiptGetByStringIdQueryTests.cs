using Moq;

using Newtonsoft.Json;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.DataAccess.Tables.RECEIPT_TABLE.Queries;

namespace XYZ.Tests.DataAccess.Tables.RECEIPT_TABLE.Queries
{
    public class ReceiptGetByStringIdQueryTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsReceipt_WhenReceiptExists()
        {
            // Arrange
            var receiptId = "receipt_123";
            var expectedReceipt = new RECEIPT { RECEIPT_ID = receiptId };

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<RECEIPT?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expectedReceipt);

            var query = new ReceiptGetByStringIdQuery(receiptId);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReceipt.RECEIPT_ID, result.RECEIPT_ID);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsNull_WhenReceiptDoesNotExist()
        {
            // Arrange
            var receiptId = "non_existing_receipt";

            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            databaseQueryMock
                .Setup(x => x.QueryFirstOrDefaultAsync<RECEIPT?>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((RECEIPT?)null);

            var query = new ReceiptGetByStringIdQuery(receiptId);

            // Act
            var result = await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_CallsQueryWithCorrectParameters()
        {
            // Arrange
            var receiptId = "receipt_456";
            var databaseQueryMock = new Mock<IDatabaseQueryExecutionLogic>();
            var query = new ReceiptGetByStringIdQuery(receiptId);

            // Act
            await query.ExecuteAsync(databaseQueryMock.Object);

            // Assert
            databaseQueryMock.Verify(x =>
                x.QueryFirstOrDefaultAsync<RECEIPT?>(It.IsAny<string>(), It.Is<object>(p =>
                    JsonConvert.SerializeObject(p) ==
                    JsonConvert.SerializeObject(new { _id = receiptId })
                )), Times.Once);
        }
    }
}
