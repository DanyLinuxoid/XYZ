using XYZ.Models.Common.Enums;
using XYZ.Web.Features.Billing.Mappers;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Tests.Mappers
{
    public class BillingOrderProcessingMapperTests
    {
        [Fact]
        public void ToOrderInfo_MapsCorrectly()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest
            {
                Description = "Test Order",
                OrderNumber = 12345,
                PayableAmount = 99.99m,
                PaymentGateway = GatewayType.PayPal,
                UserId = 67890
            };

            // Act
            var result = request.ToOrderInfo();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.OrderNumber, result.OrderNumber);
            Assert.Equal(request.PayableAmount, result.PayableAmount);
            Assert.Equal(request.PaymentGateway, result.PaymentGateway);
            Assert.Equal(request.UserId, result.UserId);
        }

        [Fact]
        public void ToOrderInfo_WithNullDescription_MapsCorrectly()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest
            {
                Description = null,
                OrderNumber = 54321,
                PayableAmount = 49.99m,
                PaymentGateway = GatewayType.PayPal,
                UserId = 98765
            };

            // Act
            var result = request.ToOrderInfo();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Description);
            Assert.Equal(request.OrderNumber, result.OrderNumber);
            Assert.Equal(request.PayableAmount, result.PayableAmount);
            Assert.Equal(request.PaymentGateway, result.PaymentGateway);
            Assert.Equal(request.UserId, result.UserId);
        }
    }
}
