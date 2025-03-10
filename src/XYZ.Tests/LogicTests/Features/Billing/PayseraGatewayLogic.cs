using Moq;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Paysera;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Tests.Logic.Features.Billing.Paysera
{
    public class PayseraGatewayLogicTests
    {
        private readonly Mock<ISimpleLogger> _simpleLoggerMock;
        private readonly Mock<IApiOrderLogic<PayseraOrderInfo, PayseraOrderResult>> _payseraApiLogicMock;
        private readonly Mock<IOrderMapperLogic<PayseraOrderInfo>> _payseraMapperLogicMock;
        private readonly Mock<IPayseraGatewayOrderSavingLogic> _payseraOrderSavingLogicMock;
        private readonly Mock<IExceptionSaverLogic> _exceptionSaverLogicMock;
        private readonly Mock<IOrderLogic> _orderLogicMock;
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;

        private readonly PayseraGatewayLogic _payseraGatewayLogic;

        public PayseraGatewayLogicTests()
        {
            _simpleLoggerMock = new Mock<ISimpleLogger>();
            _payseraApiLogicMock = new Mock<IApiOrderLogic<PayseraOrderInfo, PayseraOrderResult>>();
            _payseraMapperLogicMock = new Mock<IOrderMapperLogic<PayseraOrderInfo>>();
            _payseraOrderSavingLogicMock = new Mock<IPayseraGatewayOrderSavingLogic>();
            _exceptionSaverLogicMock = new Mock<IExceptionSaverLogic>();
            _orderLogicMock = new Mock<IOrderLogic>();
            _databaseLogicMock = new Mock<IDatabaseLogic>();

            _payseraGatewayLogic = new PayseraGatewayLogic(
                _payseraApiLogicMock.Object,
                _payseraMapperLogicMock.Object,
                _payseraOrderSavingLogicMock.Object,
                _exceptionSaverLogicMock.Object,
                _simpleLoggerMock.Object,
                _orderLogicMock.Object,
                _databaseLogicMock.Object
            );
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ReturnsError_WhenOrderIsInvalid()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 0, PayableAmount = 0, UserId = 0 };
            var mappedOrder = new PayseraOrderInfo();
            var validationResult = new OrderValidationResult
            {
                ValidationErrors = { { "OrderNumber", "Order number must be more than 0" } }
            };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);

            // Act
            var result = await _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.NotNull(result.OrderValidationResult);
            Assert.Contains(result.OrderValidationResult.ValidationErrors, e => e.Key == "OrderNumber");
            _payseraApiLogicMock.Verify(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>()), Times.Never);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ReturnsProcessedResult_WhenOrderIsValid()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };
            var processedResult = new PayseraOrderResult { IsSuccess = true, OrderStatus = OrderStatus.Completed };
            var dto = new OrderDto() { OrderNumber = 123, PayableAmount = 100, UserId = 1 };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _payseraApiLogicMock.Setup(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>())).ReturnsAsync(processedResult);
            _payseraMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _payseraApiLogicMock.Verify(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>()), Times.Once);
            _orderLogicMock.Verify(m => m.SaveOrder(It.IsAny<OrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ThrowsInvalidOperationException_WhenOrderAlreadyCompleted()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };
            var orderFull = new ORDER { ORDER_STATUS = (int)OrderStatus.Completed, PAYPAL_ORDER_ID = null };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _databaseLogicMock.Setup(m => m.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(orderFull);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo)
            );
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ThrowsException_WhenApiFails()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };
            var dto = new OrderDto() { OrderNumber = 123, PayableAmount = 100, UserId = 1 };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _payseraApiLogicMock.Setup(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>())).ThrowsAsync(new Exception("API failure"));
            _payseraMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Equal(OrderStatus.Error, result.OrderStatus);
            Assert.NotNull(result.ProblemDetails);
            _exceptionSaverLogicMock.Verify(m => m.SaveUserErrorAsync(It.IsAny<Exception>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_UpdatesOrder_WhenOrderExists()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };
            var processedResult = new PayseraOrderResult { IsSuccess = true, OrderStatus = OrderStatus.Completed };
            var orderFull = new ORDER { ORDER_STATUS = (int)OrderStatus.Processing, PAYSERA_ORDER_ID = 1, USER_ID = 1 };
            var dto = new OrderDto() { OrderNumber = 123, PayableAmount = 100, UserId = 1 };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _payseraApiLogicMock.Setup(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>())).ReturnsAsync(processedResult);
            _databaseLogicMock.Setup(m => m.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(orderFull);
            _payseraMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _payseraApiLogicMock.Verify(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>()), Times.Once);
            _orderLogicMock.Verify(m => m.UpdateOrder(It.IsAny<OrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_SavesOrder_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };
            var processedResult = new PayseraOrderResult { IsSuccess = true, OrderStatus = OrderStatus.Completed };
            var orderFull = (ORDER?)null; // Order does not exist
            var dto = new OrderDto() { OrderNumber = 123, PayableAmount = 100, UserId = 1 };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _payseraApiLogicMock.Setup(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>())).ReturnsAsync(processedResult);
            _databaseLogicMock.Setup(m => m.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(orderFull);
            _payseraMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _payseraApiLogicMock.Verify(m => m.GetProcessedOrderResultAsync(It.IsAny<PayseraOrderInfo>()), Times.Once);
            _orderLogicMock.Verify(m => m.SaveOrder(It.IsAny<OrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ReturnsError_WhenDatabaseFails()
        {
            // Arrange
            var orderInfo = new OrderInfo { OrderNumber = 123, PayableAmount = 100, UserId = 1 };
            var mappedOrder = new PayseraOrderInfo { OrderNumber = orderInfo.OrderNumber, PayableAmount = orderInfo.PayableAmount, UserId = 1 };

            _payseraMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _databaseLogicMock.Setup(m => m.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _payseraGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo));

            // Assert
            Assert.Equal("Database error", exception.Message);
        }
    }
}
