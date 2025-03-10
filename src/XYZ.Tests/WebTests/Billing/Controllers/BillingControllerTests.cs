using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Validation;
using XYZ.Models.Features.Billing.WebResults;
using XYZ.Web.Features.Billing.Controllers;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Tests.WebTests.Billing.Controllers
{
    public class BillingControllerTests
    {
        private readonly Mock<IBillingLogic> _billingServiceMock;
        private readonly BillingController _controller;

        public BillingControllerTests()
        {
            _billingServiceMock = new Mock<IBillingLogic>();
            _controller = new BillingController(_billingServiceMock.Object);
        }

        [Fact]
        public async Task ProcessOrder_WhenValidationErrors_ReturnsBadRequest()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest();
            var fakeResult = new OrderProcessingResult
            {
                OrderValidationResult = new OrderValidationResult
                {
                    ValidationErrors = new Dictionary<string, string> { { "somefield", "someerror" } }
                }
            };

            _billingServiceMock
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(fakeResult);

            // Act
            var result = await _controller.ProcessOrder(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            _billingServiceMock.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOrder_When3rdPartyFails_Returns502BadGateway()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest();
            var fakeResult = new OrderProcessingResult
            {
                ProblemDetails = new ProblemDetailed("Service Unavailable", "Payment provider is down")
            };

            _billingServiceMock
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(fakeResult);

            // Act
            var result = await _controller.ProcessOrder(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status502BadGateway, objectResult.StatusCode);

            _billingServiceMock.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOrder_WhenSuccessful_ReturnsOk()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest();
            var fakeResult = new OrderProcessingResult();

            _billingServiceMock
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()))
                .ReturnsAsync(fakeResult);

            // Act
            var result = await _controller.ProcessOrder(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            _billingServiceMock.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderInfo>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderInfo_WhenOrderNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new GetOrderInfoRequest { UserId = 1, OrderNumber = 123 };

            _billingServiceMock
                .Setup(x => x.GetOrderInfoAsync(request.UserId, request.OrderNumber))
                .ReturnsAsync((OrderDto?)null);

            // Act
            var result = await _controller.GetOrderInfo(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _billingServiceMock.Verify(x => x.GetOrderInfoAsync(request.UserId, request.OrderNumber), Times.Once);
        }

        [Fact]
        public async Task GetOrderInfo_WhenOrderExists_ReturnsOk()
        {
            // Arrange
            var request = new GetOrderInfoRequest { UserId = 1, OrderNumber = 123 };
            var fakeOrder = new OrderDto();

            _billingServiceMock
                .Setup(x => x.GetOrderInfoAsync(request.UserId, request.OrderNumber))
                .ReturnsAsync(fakeOrder);

            // Act
            var result = await _controller.GetOrderInfo(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            _billingServiceMock.Verify(x => x.GetOrderInfoAsync(request.UserId, request.OrderNumber), Times.Once);
        }

        [Fact]
        public async Task GetReceiptInfo_WhenReceiptNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new GetReceiptInfoRequest { ReceiptId = "456" };

            _billingServiceMock
                .Setup(x => x.GetReceiptAsync(request.ReceiptId))
                .ReturnsAsync((ReceiptDto?)null);

            // Act
            var result = await _controller.GetReceiptInfo(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _billingServiceMock.Verify(x => x.GetReceiptAsync(request.ReceiptId), Times.Once);
        }

        [Fact]
        public async Task GetReceiptInfo_WhenReceiptExists_ReturnsOk()
        {
            // Arrange
            var request = new GetReceiptInfoRequest { ReceiptId = "456" };
            var fakeReceipt = new ReceiptDto();

            _billingServiceMock
                .Setup(x => x.GetReceiptAsync(request.ReceiptId))
                .ReturnsAsync(fakeReceipt);

            // Act
            var result = await _controller.GetReceiptInfo(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            _billingServiceMock.Verify(x => x.GetReceiptAsync(request.ReceiptId), Times.Once);
        }
    }
}
