using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Tests.Logic.Features.Billing.Mappers
{
    public class OrderDtoMapperTests
    {
        [Fact]
        public void ToDbo_ValidOrderDto_ReturnsMappedOrder()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                PayableAmount = 100.0m,
                Description = "Test description",
                OrderNumber = 12345,
                UserId = 67890,
                OrderStatus = OrderStatus.Completed,
                PaypalOrderId = 54321,
                PayseraOrderId = 98765
            };

            // Act
            var result = orderDto.ToDbo();

            // Assert
            Assert.Equal(orderDto.PayableAmount, result.PAYABLE_AMOUNT);
            Assert.Equal(orderDto.Description, result.DESCRIPTION);
            Assert.Equal(orderDto.OrderNumber, result.ORDER_NUMBER);
            Assert.Equal(orderDto.UserId, result.USER_ID);
            Assert.Equal((int)orderDto.OrderStatus, result.ORDER_STATUS);
            Assert.Equal(orderDto.PaypalOrderId, result.PAYPAL_ORDER_ID);
            Assert.Equal(orderDto.PayseraOrderId, result.PAYSERA_ORDER_ID);
        }

        [Fact]
        public void ToDto_ValidOrder_ReturnsMappedOrderDto()
        {
            // Arrange
            var order = new ORDER
            {
                PAYABLE_AMOUNT = 100.0m,
                DESCRIPTION = "Test description",
                ORDER_NUMBER = 12345,
                USER_ID = 67890,
                ORDER_STATUS = (int)OrderStatus.Completed,
                PAYPAL_ORDER_ID = 54321,
                PAYSERA_ORDER_ID = 98765
            };

            // Act
            var result = order.ToDto();

            // Assert
            Assert.Equal(order.PAYABLE_AMOUNT, result.PayableAmount);
            Assert.Equal(order.DESCRIPTION, result.Description);
            Assert.Equal(order.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(order.USER_ID, result.UserId);
            Assert.Equal((OrderStatus)order.ORDER_STATUS, result.OrderStatus);
            Assert.Equal(order.PAYPAL_ORDER_ID, result.PaypalOrderId);
            Assert.Equal(order.PAYSERA_ORDER_ID, result.PayseraOrderId);
        }

        [Fact]
        public void ToDbo_EmptyOrderDto_ReturnsOrderWithDefaultValues()
        {
            // Arrange
            var orderDto = new OrderDto();

            // Act
            var result = orderDto.ToDbo();

            // Assert
            Assert.Equal(orderDto.PayableAmount, result.PAYABLE_AMOUNT);
            Assert.Equal(orderDto.Description, result.DESCRIPTION);
            Assert.Equal(orderDto.OrderNumber, result.ORDER_NUMBER);
            Assert.Equal(orderDto.UserId, result.USER_ID);
            Assert.Equal((int)orderDto.OrderStatus, result.ORDER_STATUS);
            Assert.Equal(orderDto.PaypalOrderId, result.PAYPAL_ORDER_ID);
            Assert.Equal(orderDto.PayseraOrderId, result.PAYSERA_ORDER_ID);
        }

        [Fact]
        public void ToDto_EmptyOrder_ReturnsOrderDtoWithDefaultValues()
        {
            // Arrange
            var order = new ORDER();

            // Act
            var result = order.ToDto();

            // Assert
            Assert.Equal(order.PAYABLE_AMOUNT, result.PayableAmount);
            Assert.Equal(order.DESCRIPTION, result.Description);
            Assert.Equal(order.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(order.USER_ID, result.UserId);
            Assert.Equal((OrderStatus)order.ORDER_STATUS, result.OrderStatus);
            Assert.Equal(order.PAYPAL_ORDER_ID, result.PaypalOrderId);
            Assert.Equal(order.PAYSERA_ORDER_ID, result.PayseraOrderId);
        }
    }
}
