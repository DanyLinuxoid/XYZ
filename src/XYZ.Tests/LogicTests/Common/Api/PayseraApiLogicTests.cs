using XYZ.Logic.Common.Api.Paysera;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;

namespace XYZ.Tests.Logic.Common.Api.Paysera
{
    public class PayseraApiLogicTests
    {
        private readonly PayseraApiLogic _payseraApiLogic;

        public PayseraApiLogicTests()
        {
            _payseraApiLogic = new PayseraApiLogic();
        }

        [Fact]
        public async Task GetProcessedOrderResultAsync_ReturnsFailure_WhenResultIsFailure()
        {
            // Arrange
            var payseraOrderInfo = new PayseraOrderInfo();
            var expectedResult = new PayseraOrderResult
            {
                IsSuccess = false,
                OrderStatus = OrderStatus.Failed,
                ProblemDetails = new ProblemDetailed("Paysera Api failed to process order null", "Technical maintenance is currently underway")
            };

            // Act
            var result = await _payseraApiLogic.GetProcessedOrderResultAsync(payseraOrderInfo);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.False(Guid.TryParse(expectedResult.GatewayTransactionId, out var _));
            Assert.Equal(expectedResult.OrderStatus, result.OrderStatus);
            Assert.Equal(expectedResult.ProblemDetails.Details, result.ProblemDetails?.Details);
        }
    }
}
