using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.Logic.Features.Billing.Common;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Tests.Logic.Features.Billing.Common
{
    public class OrderLogicTests
    {
        [Fact]
        public async Task GetOrderAsync_ValidOrder_ReturnsOrderDto()
        {
            // Arrange
            var userId = 123;
            var orderNumber = 456;
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var order = new ORDER
            {
                ORDER_NUMBER = orderNumber,
                USER_ID = userId,
                DESCRIPTION = "Test Order",
                ORDER_STATUS = (int)XYZ.Models.Common.Enums.OrderStatus.Completed,
                PAYABLE_AMOUNT = 100.00M
            };
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>()))
                .ReturnsAsync(order);

            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act
            var result = await orderLogic.GetOrderAsync(userId, orderNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.ORDER_NUMBER, result.OrderNumber);
            Assert.Equal(order.USER_ID, result.UserId);
            Assert.Equal(order.DESCRIPTION, result.Description);
            Assert.Equal((XYZ.Models.Common.Enums.OrderStatus)order.ORDER_STATUS, result.OrderStatus);
            Assert.Equal(order.PAYABLE_AMOUNT, result.PayableAmount);
        }

        [Fact]
        public async Task GetOrderAsync_InvalidOrder_ReturnsNull()
        {
            // Arrange
            var userId = 123;
            var orderNumber = 456;
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>()))
                .ReturnsAsync((ORDER?)null);

            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act
            var result = await orderLogic.GetOrderAsync(userId, orderNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveOrder_ValidOrder_ReturnsOrderId()
        {
            // Arrange
            var billingOrder = new OrderDto
            {
                OrderNumber = 456,
                UserId = 123,
                Description = "Test Order",
                OrderStatus = Models.Common.Enums.OrderStatus.Completed,
                PayableAmount = 100.00M
            };
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.CommandAsync(It.IsAny<ORDER_CUD>(), CommandTypes.Create, It.IsAny<ORDER>()))
                .ReturnsAsync(1);

            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act
            var result = await orderLogic.SaveOrder(billingOrder);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void SaveOrder_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await orderLogic.SaveOrder(null));
            Assert.Equal("billingOrder", exception?.Result?.ParamName);
        }

        [Fact]
        public async Task UpdateOrder_ValidOrder_UpdatesSuccessfully()
        {
            // Arrange
            var billingOrder = new OrderDto
            {
                OrderNumber = 456,
                UserId = 123,
                Description = "Updated Order Description",
                OrderStatus = XYZ.Models.Common.Enums.OrderStatus.Completed,
                PayableAmount = 150.00M
            };
            var existingOrder = new ORDER
            {
                ORDER_NUMBER = 456,
                USER_ID = 123,
                DESCRIPTION = "Old Order Description",
                ORDER_STATUS = (int)XYZ.Models.Common.Enums.OrderStatus.Completed,
                PAYABLE_AMOUNT = 100.00M
            };
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>()))
                .ReturnsAsync(existingOrder);
            mockDatabaseLogic.Setup(db => db.CommandAsync(It.IsAny<ORDER_CUD>(), CommandTypes.Update, It.IsAny<ORDER>()))
                .ReturnsAsync(1);

            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act
            await orderLogic.UpdateOrder(billingOrder);

            // Assert
            mockDatabaseLogic.Verify(db => db.CommandAsync(It.IsAny<ORDER_CUD>(), CommandTypes.Update, It.IsAny<ORDER>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrder_OrderNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var billingOrder = new OrderDto
            {
                OrderNumber = 456,
                UserId = 123,
                Description = "Updated Order Description",
                OrderStatus = XYZ.Models.Common.Enums.OrderStatus.Completed,
                PayableAmount = 150.00M
            };
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            mockDatabaseLogic.Setup(db => db.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>()))
                .ReturnsAsync((ORDER?)null);

            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await orderLogic.UpdateOrder(billingOrder));
            Assert.Equal($"Order with id {billingOrder.OrderNumber} for user {billingOrder.UserId} not found for update", exception.Message);
        }

        [Fact]
        public void UpdateOrder_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDatabaseLogic = new Mock<IDatabaseLogic>();
            var orderLogic = new OrderLogic(mockDatabaseLogic.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await orderLogic.UpdateOrder(null));
            Assert.Equal("billingOrder", exception?.Result?.ParamName);
        }
    }
}
