using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.DataAccess.Tables.RECEIPT_TABLE.Queries;
using XYZ.Logic.Features.Billing.Common;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Tests.Logic.Features.Billing.Common
{
    public class ReceiptLogicTests
    {
        [Fact]
        public async Task GetReceipt_ValidReceiptId_ReturnsReceiptDto()
        {
            // Arrange
            var receiptId = "valid_receipt_id";
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var receipt = new RECEIPT
            {
                RECEIPT_ID = "1",
                ORDER_NUMBER = 123,
                USER_ID = 456,
                GATEWAY_TRANSACTION_ID = "gateway_transaction_id"
            };
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<ReceiptGetByStringIdQuery>()))
                .ReturnsAsync(receipt);

            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act
            var result = await receiptLogic.GetReceipt(receiptId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receipt.RECEIPT_ID, result.ReceiptId);
            Assert.Equal(receipt.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(receipt.USER_ID, result.UserId);
            Assert.Equal(receipt.GATEWAY_TRANSACTION_ID, result.GatewayTransactionId);
        }

        [Fact]
        public async Task GetReceipt_InvalidReceiptId_ReturnsNull()
        {
            // Arrange
            var receiptId = "invalid_receipt_id";
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<ReceiptGetByStringIdQuery>()))
                .ReturnsAsync((RECEIPT?)null);

            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act
            var result = await receiptLogic.GetReceipt(receiptId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetReceipt_EmptyReceiptId_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await receiptLogic.GetReceipt(string.Empty));
            Assert.Equal("receiptId", exception?.Result?.ParamName);
        }

        [Fact]
        public void GetReceipt_NullReceiptId_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await receiptLogic.GetReceipt(string.Empty));
            Assert.Equal("receiptId", exception?.Result?.ParamName);
        }

        [Fact]
        public async Task SaveReceipt_ValidReceiptDto_ReturnsReceiptId()
        {
            // Arrange
            var receiptDto = new ReceiptDto
            {
                OrderNumber = 123,
                UserId = 456,
                ReceiptId = "1",
                GatewayTransactionId = "gateway_transaction_id"
            };
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.CommandAsync(It.IsAny<RECEIPT_CUD>(), CommandTypes.Create, It.IsAny<RECEIPT>()))
                .ReturnsAsync(1);

            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act
            var result = await receiptLogic.SaveReceipt(receiptDto);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void SaveReceipt_NullReceiptDto_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var receiptLogic = new ReceiptLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await receiptLogic.SaveReceipt(null));
            Assert.Equal("receipt", exception?.Result?.ParamName);
        }
    }
}
