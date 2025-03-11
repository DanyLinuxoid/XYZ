using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Tests.LogicTests.Features.Billing.MapperTests
{
    public class ReceiptMapperTests
    {
        [Fact]
        public void ToDto_ValidReceipt_ReturnsMappedReceiptDto()
        {
            // Arrange
            var receipt = new RECEIPT
            {
                ORDER_NUMBER = 12345,
                USER_ID = 67890,
                RECEIPT_ID = "54321",
                GATEWAY_TRANSACTION_ID = "TXN12345"
            };

            // Act
            var result = receipt.ToDto();

            // Assert
            Assert.Equal(receipt.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(receipt.USER_ID, result.UserId);
            Assert.Equal(receipt.RECEIPT_ID, result.ReceiptId);
            Assert.Equal(receipt.GATEWAY_TRANSACTION_ID, result.GatewayTransactionId);
        }

        [Fact]
        public void ToDto_EmptyReceipt_ReturnsReceiptDtoWithDefaultValues()
        {
            // Arrange
            var receipt = new RECEIPT();

            // Act
            var result = receipt.ToDto();

            // Assert
            Assert.Equal(receipt.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(receipt.USER_ID, result.UserId);
            Assert.Equal(receipt.RECEIPT_ID, result.ReceiptId);
            Assert.Equal(receipt.GATEWAY_TRANSACTION_ID, result.GatewayTransactionId);
        }

        [Fact]
        public void ToDbo_ValidReceiptDto_ReturnsMappedReceipt()
        {
            // Arrange
            var receiptDto = new ReceiptDto
            {
                OrderNumber = 12345,
                UserId = 67890,
                ReceiptId = "54321",
                GatewayTransactionId = "TXN12345"
            };

            // Act
            var result = receiptDto.ToDbo();

            // Assert
            Assert.Equal(receiptDto.OrderNumber, result.ORDER_NUMBER);
            Assert.Equal(receiptDto.UserId, result.USER_ID);
            Assert.Equal(receiptDto.ReceiptId, result.RECEIPT_ID);
            Assert.Equal(receiptDto.GatewayTransactionId, result.GATEWAY_TRANSACTION_ID);
        }

        [Fact]
        public void ToDbo_EmptyReceiptDto_ReturnsReceiptWithDefaultValues()
        {
            // Arrange
            var receiptDto = new ReceiptDto();

            // Act
            var result = receiptDto.ToDbo();

            // Assert
            Assert.Equal(receiptDto.OrderNumber, result.ORDER_NUMBER);
            Assert.Equal(receiptDto.UserId, result.USER_ID);
            Assert.Equal(receiptDto.ReceiptId, result.RECEIPT_ID);
            Assert.Equal(receiptDto.GatewayTransactionId, result.GATEWAY_TRANSACTION_ID);
        }
    }
}
