using Moq;

using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Data.Order.Order;
using XYZ.Models.Features.Billing.Validation;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Tests.Logic.Features.Billing
{
    public class BillingLogicTests
    {
        private readonly Mock<IBillingGatewayFactory> _billingGatewayFactoryMock;
        private readonly Mock<IUserLogic> _userLogicMock;
        private readonly Mock<IReceiptLogic> _receiptLogicMock;
        private readonly Mock<IOrderLogic> _orderLogicMock;
        private readonly BillingLogic _billingLogic;

        public BillingLogicTests()
        {
            _billingGatewayFactoryMock = new Mock<IBillingGatewayFactory>();
            _userLogicMock = new Mock<IUserLogic>();
            _receiptLogicMock = new Mock<IReceiptLogic>();
            _orderLogicMock = new Mock<IOrderLogic>();
            _billingLogic = new BillingLogic(
                _billingGatewayFactoryMock.Object,
                _userLogicMock.Object,
                _receiptLogicMock.Object,
                _orderLogicMock.Object);
        }

        [Fact]
        public async Task GetReceiptAsync_ReturnsReceipt_WhenReceiptExists()
        {
            // Arrange
            var receiptId = "receipt123";
            var expectedReceipt = new ReceiptDto
            {
                ReceiptId = receiptId,
                UserId = 12345,
                OrderNumber = 123,
                GatewayTransactionId = "gateway-transaction-id"
            };

            _receiptLogicMock.Setup(x => x.GetReceipt(receiptId))
                .ReturnsAsync(expectedReceipt);

            // Act
            var result = await _billingLogic.GetReceiptAsync(receiptId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receiptId, result.ReceiptId);
            Assert.Equal(expectedReceipt.UserId, result.UserId);
            Assert.Equal(expectedReceipt.OrderNumber, result.OrderNumber);
        }

        [Fact]
        public async Task GetOrderInfoAsync_ReturnsOrderInfo_WhenOrderExists()
        {
            // Arrange
            var userId = 12345;
            var orderId = 67890;
            var expectedOrder = new OrderDto
            {
                OrderNumber = 123,
                UserId = userId,
                Description = "12",
                OrderStatus = OrderStatus.Completed,
                PaypalOrderId = 6,
            };

            _orderLogicMock.Setup(x => x.GetOrderAsync(userId, orderId))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _billingLogic.GetOrderInfoAsync(userId, orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.OrderNumber, result.OrderNumber);
            Assert.Equal(expectedOrder.UserId, result.UserId);
            Assert.Equal(expectedOrder.Description, result.Description);
            Assert.Equal(expectedOrder.OrderStatus, result.OrderStatus);
            Assert.Equal(expectedOrder.PaypalOrderId, result.PaypalOrderId);
        }

        [Fact]
        public async Task ProcessOrderAsync_ReturnsOrderProcessingResult_WhenUserNotFound()
        {
            // Arrange
            var orderInfo = new OrderInfo
            {
                OrderNumber = 123,
                UserId = 22,
                Description = "12",
                OrderStatus = OrderStatus.Completed,
            };
            _userLogicMock.Setup(x => x.GetUserInfo(orderInfo.UserId))
                .ReturnsAsync((UserDto?)null);

            // Act
            var result = await _billingLogic.ProcessOrderAsync(orderInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Receipt);
            Assert.NotNull(result.ProblemDetails);
            Assert.Equal("User not found", result.ProblemDetails.Title);
        }

        [Fact]
        public async Task ProcessOrderAsync_ReturnsOrderProcessingResult_WhenValidationFails()
        {
            // Arrange
            var orderInfo = new OrderInfo
            {
                OrderNumber = 123,
                UserId = 22,
                Description = "12",
                OrderStatus = OrderStatus.Completed,
            };

            var orderResult = new OrderResult
            {
                OrderValidationResult = new OrderValidationResult
                {
                    ValidationErrors = new Dictionary<string, string>() { { "one", "two" } } 
                }
            };

            var gatewayLogicMock = new Mock<IPaymentGatewayLogic>();
            gatewayLogicMock.Setup(g => g.GetGatewayOrderProcessResultAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(orderResult);
            _billingGatewayFactoryMock.Setup(x => x.GetPaymentGateway(It.IsAny<PaymentGatewayType>())).Returns(gatewayLogicMock.Object);
            _userLogicMock.Setup(x => x.GetUserInfo(It.IsAny<long>())).Returns(Task.FromResult<UserDto?>(new UserDto()));

            // Act
            var result = await _billingLogic.ProcessOrderAsync(orderInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Receipt);
            Assert.NotNull(result.OrderValidationResult);
            Assert.Single(result.OrderValidationResult.ValidationErrors);
        }

        [Fact]
        public async Task ProcessOrderAsync_ReturnsOrderProcessingResult_WhenOrderFailed()
        {
            // Arrange
            var orderInfo = new OrderInfo
            {
                OrderNumber = 123,
                UserId = 22,
                Description = "12",
                OrderStatus = OrderStatus.Completed,
            };

            var orderResult = new OrderResult
            {
                IsSuccess = false,
                ProblemDetails = new ProblemDetailed("GatewayError", "Payment gateway failed")
            };

            var gatewayLogicMock = new Mock<IPaymentGatewayLogic>();
            gatewayLogicMock.Setup(g => g.GetGatewayOrderProcessResultAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(orderResult);
            _billingGatewayFactoryMock.Setup(x => x.GetPaymentGateway(It.IsAny<PaymentGatewayType>())).Returns(gatewayLogicMock.Object);
            _userLogicMock.Setup(x => x.GetUserInfo(It.IsAny<long>())).Returns(Task.FromResult<UserDto?>(new UserDto()));

            // Act
            var result = await _billingLogic.ProcessOrderAsync(orderInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Receipt);
            Assert.NotNull(result.ProblemDetails);
            Assert.Equal("Payment gateway failed", result.ProblemDetails.Details);
        }

        [Fact]
        public async Task ProcessOrderAsync_ReturnsOrderProcessingResult_WhenOrderSucceeds()
        {
            // Arrange
            var orderInfo = new OrderInfo
            {
                OrderNumber = 123,
                UserId = 22,
                Description = "12",
                OrderStatus = OrderStatus.Completed,
            };

            var orderResult = new OrderResult
            {
                IsSuccess = true,
                GatewayTransactionId = "gateway-transaction-id"
            };

            var gatewayLogicMock = new Mock<IPaymentGatewayLogic>();
            gatewayLogicMock.Setup(g => g.GetGatewayOrderProcessResultAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(orderResult);
            _billingGatewayFactoryMock.Setup(x => x.GetPaymentGateway(It.IsAny<PaymentGatewayType>())).Returns(gatewayLogicMock.Object);
            _userLogicMock.Setup(x => x.GetUserInfo(It.IsAny<long>())).Returns(Task.FromResult<UserDto?>(new UserDto()));

            var expectedReceipt = new ReceiptDto
            {
                UserId = orderInfo.UserId,
                OrderNumber = orderInfo.OrderNumber,
                GatewayTransactionId = orderResult.GatewayTransactionId,
                ReceiptId = "generated-receipt-id"
            };

            _receiptLogicMock.Setup(x => x.SaveReceipt(It.IsAny<ReceiptDto>()));

            // Act
            var result = await _billingLogic.ProcessOrderAsync(orderInfo);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Receipt);
            Assert.Equal(expectedReceipt.UserId, result.Receipt.UserId);
            Assert.Equal(expectedReceipt.OrderNumber, result.Receipt.OrderNumber);
            Assert.Equal(expectedReceipt.GatewayTransactionId, result.Receipt.GatewayTransactionId);
        }
    }
}
