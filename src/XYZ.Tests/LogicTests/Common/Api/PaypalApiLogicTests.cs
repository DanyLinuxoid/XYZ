using XYZ.Logic.Common.Api.Paypal;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;

namespace XYZ.Tests.Logic.Common.Api.Paypal
{
    public class PaypalApiLogicTests
    {
        private readonly PaypalApiLogic _paypalApiLogic;

        public PaypalApiLogicTests()
        {
            _paypalApiLogic = new PaypalApiLogic();
        }

        [Fact]
        public async Task GetProcessedOrderResultAsync_ReturnsSuccess_WhenResultIsSuccess()
        {
            // Arrange
            var paypalOrderInfo = new PaypalOrderInfo();
            var expectedResult = new PaypalOrderResult
            {
                IsSuccess = true,
                OrderStatus = OrderStatus.Completed
            };

            // Act
            var result = await _paypalApiLogic.GetProcessedOrderResultAsync(paypalOrderInfo);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.GatewayTransactionId));
            Assert.True(Guid.TryParse(result.GatewayTransactionId, out var _));
            Assert.Equal(expectedResult.OrderStatus, result.OrderStatus);
        }
    }
}
