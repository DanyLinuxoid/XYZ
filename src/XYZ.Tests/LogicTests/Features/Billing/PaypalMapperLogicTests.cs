using XYZ.Logic.Features.Billing.Paypal;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;

namespace XYZ.Tests.Logic.Features.Billing.Paypal
{
    public class PaypalMapperLogicTests
    {
        private readonly PaypalMapperLogic _paypalMapperLogic;

        public PaypalMapperLogicTests()
        {
            _paypalMapperLogic = new PaypalMapperLogic();
        }

        [Fact]
        public void ToMappedOrderInfo_MapsCorrectly_From_OrderInfo()
        {
            // Arrange
            var orderInfo = new OrderInfo
            {
                OrderNumber = 12345,
                UserId = 67890,
                Description = "Test Order",
                PayableAmount = 100.50m,
                OrderStatus = OrderStatus.Completed
            };

            // Act
            var result = _paypalMapperLogic.ToMappedOrderInfo(orderInfo);

            // Assert
            Assert.Equal(orderInfo.OrderNumber, result.OrderNumber);
            Assert.Equal(orderInfo.UserId, result.UserId);
            Assert.Equal(orderInfo.Description, result.Description);
            Assert.Equal(orderInfo.PayableAmount, result.PayableAmount);
            Assert.Equal(GatewayType.PayPal, result.PaymentGateway);
            Assert.Equal(orderInfo.OrderStatus, result.OrderStatus);
        }

        [Fact]
        public void ToMappedOrderDto_MapsCorrectly_From_PaypalOrderInfo()
        {
            // Arrange
            var paypalOrderInfo = new PaypalOrderInfo
            {
                OrderNumber = 12345,
                UserId = 67890,
                Description = "Test Order",
                PayableAmount = 100.50m,
                OrderStatus = OrderStatus.Completed
            };

            // Act
            var result = _paypalMapperLogic.ToMappedOrderDto(paypalOrderInfo);

            // Assert
            Assert.Equal(paypalOrderInfo.OrderNumber, result.OrderNumber);
            Assert.Equal(paypalOrderInfo.UserId, result.UserId);
            Assert.Equal(paypalOrderInfo.Description, result.Description);
            Assert.Equal(paypalOrderInfo.PayableAmount, result.PayableAmount);
            Assert.Equal(paypalOrderInfo.OrderStatus, result.OrderStatus);
        }
    }
}
