using Moq;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Paypal;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Tests.Logic.Features.Billing.Paypal
{
    public class PaypalGatewayLogicTests
    {
        private readonly Mock<ISimpleLogger> _loggerMock;
        private readonly Mock<IApiOrderLogic<PaypalOrderInfo, PaypalOrderResult>> _paypalApiLogicMock;
        private readonly Mock<IOrderMapperLogic<PaypalOrderInfo>> _paypalMapperLogicMock;
        private readonly Mock<IGatewayOrderSavingLogic<PAYPAL_GATEWAY_ORDER>> _paypalOrderSavingLogicMock;
        private readonly Mock<IExceptionSaverLogic> _exceptionSaverLogicMock;
        private readonly Mock<IOrderLogic> _orderLogicMock;
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly PaypalGatewayLogic _paypalGatewayLogic;

        public PaypalGatewayLogicTests()
        {
            _loggerMock = new Mock<ISimpleLogger>();
            _paypalApiLogicMock = new Mock<IApiOrderLogic<PaypalOrderInfo, PaypalOrderResult>>();
            _paypalMapperLogicMock = new Mock<IOrderMapperLogic<PaypalOrderInfo>>();
            _paypalOrderSavingLogicMock = new Mock<IGatewayOrderSavingLogic<PAYPAL_GATEWAY_ORDER>>();
            _exceptionSaverLogicMock = new Mock<IExceptionSaverLogic>();
            _orderLogicMock = new Mock<IOrderLogic>();
            _databaseLogicMock = new Mock<IDatabaseLogic>();

            _paypalGatewayLogic = new PaypalGatewayLogic(
                _loggerMock.Object,
                _paypalApiLogicMock.Object,
                _paypalMapperLogicMock.Object,
                _paypalOrderSavingLogicMock.Object,
                _exceptionSaverLogicMock.Object,
                _loggerMock.Object,
                _orderLogicMock.Object,
                _databaseLogicMock.Object
            );
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ValidOrder_ReturnsOrderResult()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var orderResult = new PaypalOrderResult { OrderStatus = OrderStatus.Completed };
            var dto = new OrderDto { UserId = 1, OrderNumber = 100, PayableAmount = 100 };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _paypalApiLogicMock.Setup(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>())).ReturnsAsync(orderResult);
            _orderLogicMock.Setup(o => o.SaveOrder(It.IsAny<OrderDto>())).Returns(Task.FromResult((long)0));
            _databaseLogicMock.Setup(d => d.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync((ORDER?)null);
            _paypalOrderSavingLogicMock.Setup(p => p.SaveOrder(new PAYPAL_GATEWAY_ORDER())).ReturnsAsync(1);
            _paypalMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _paypalApiLogicMock.Verify(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>()), Times.Once);
            _orderLogicMock.Verify(o => o.SaveOrder(It.IsAny<OrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ValidationFails_ReturnsValidationResult()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 0, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 0, OrderNumber = 100, PayableAmount = 100 };
            var validationResult = new PaypalOrderResult
            {
                OrderValidationResult = new OrderValidationResult
                {
                    ValidationErrors = { { "UserId", "User id must be more than 0" } }
                }
            };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);

            // Act
            var result = await _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Contains(result.OrderValidationResult.ValidationErrors, e => e.Key == "UserId");
            _paypalApiLogicMock.Verify(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>()), Times.Never);
            _orderLogicMock.Verify(o => o.SaveOrder(It.IsAny<OrderDto>()), Times.Never);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ExistingOrderWithPaypalId_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var existingOrder = new ORDER { PAYSERA_ORDER_ID = 1 };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _databaseLogicMock.Setup(d => d.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(existingOrder);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo));
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ExistingOrderCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var existingOrder = new ORDER { ORDER_STATUS = (int)OrderStatus.Completed };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _databaseLogicMock.Setup(d => d.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(existingOrder);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo));
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ExceptionInProcessing_ReturnsErrorResult()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var dto = new OrderDto { UserId = 1, OrderNumber = 100, PayableAmount = 100 };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _paypalApiLogicMock.Setup(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>())).ThrowsAsync(new Exception("Payment failed"));
            _paypalMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Equal(OrderStatus.Error, result.OrderStatus);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_ValidOrderAndExistingOrder_ReturnsUpdatedOrderResult()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var existingOrder = new ORDER { PAYPAL_ORDER_ID = 1, ORDER_STATUS = (int)OrderStatus.Processing };
            var orderResult = new PaypalOrderResult { OrderStatus = OrderStatus.Completed };
            var dto = new OrderDto { UserId = 1, OrderNumber = 100, PayableAmount = 100 };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _paypalApiLogicMock.Setup(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>())).ReturnsAsync(orderResult);
            _orderLogicMock.Setup(o => o.UpdateOrder(It.IsAny<OrderDto>())).Returns(Task.CompletedTask);
            _databaseLogicMock.Setup(d => d.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync(existingOrder);
            _paypalMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);

            // Act
            var result = await _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _orderLogicMock.Verify(o => o.UpdateOrder(It.IsAny<OrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetGatewayOrderProcessResultAsync_SuccessfulOrderSave_ReturnsValidResult()
        {
            // Arrange
            var orderInfo = new OrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var mappedOrder = new PaypalOrderInfo { UserId = 1, OrderNumber = 100, PayableAmount = 100 };
            var orderResult = new PaypalOrderResult { OrderStatus = OrderStatus.Completed };
            var dto = new OrderDto { UserId = 1, OrderNumber = 100, PayableAmount = 100 };

            _paypalMapperLogicMock.Setup(m => m.ToMappedOrderInfo(It.IsAny<OrderInfo>())).Returns(mappedOrder);
            _paypalApiLogicMock.Setup(a => a.GetProcessedOrderResultAsync(It.IsAny<PaypalOrderInfo>())).ReturnsAsync(orderResult);
            _databaseLogicMock.Setup(d => d.QueryAsync(It.IsAny<OrderByOrderNumberAndUserIdGetQuery>())).ReturnsAsync((ORDER?)null);
            _paypalOrderSavingLogicMock
                .Setup(p => p.SaveOrder(It.IsAny<PAYPAL_GATEWAY_ORDER>()))
                .ReturnsAsync(1); _paypalMapperLogicMock.Setup(x => x.ToMappedOrderDto(mappedOrder)).Returns(dto);
            _orderLogicMock.Setup(o => o.SaveOrder(It.IsAny<OrderDto>())).ReturnsAsync(1);

            // Act
            var result = await _paypalGatewayLogic.GetGatewayOrderProcessResultAsync(orderInfo);

            // Assert
            Assert.Equal(OrderStatus.Completed, result.OrderStatus);
            _paypalOrderSavingLogicMock.Verify(p => p.SaveOrder(It.IsAny<PAYPAL_GATEWAY_ORDER>()), Times.Once);
        }
    }
}
