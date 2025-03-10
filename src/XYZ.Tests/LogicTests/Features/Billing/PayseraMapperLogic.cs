using XYZ.Logic.Features.Billing.Paysera;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;

namespace XYZ.Tests.LogicTests.Features.Billing
{
    public class PayseraMapperLogicTests
    {
        private readonly PayseraMapperLogic _payseraMapperLogic;

        public PayseraMapperLogicTests()
        {
            _payseraMapperLogic = new PayseraMapperLogic();
        }

        [Fact]
        public void ToMappedOrderInfo_ReturnsMappedOrderInfo_WhenOrderInfoIsValid()
        {
            // Arrange
            var order = new OrderInfo
            {
                OrderNumber = 123,
                UserId = 12345,
                Description = "Sample Order",
                PayableAmount = 100.50m,
                PaymentGateway = PaymentGatewayType.Paysera
            };

            // Act
            var result = _payseraMapperLogic.ToMappedOrderInfo(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderNumber, result.OrderNumber);
            Assert.Equal(order.UserId, result.UserId);
            Assert.Equal(order.Description, result.Description);
            Assert.Equal(order.PayableAmount, result.PayableAmount);
            Assert.Equal(PaymentGatewayType.Paysera, result.PaymentGateway);
        }

        [Fact]
        public void ToMappedOrderDto_ReturnsMappedOrderDto_WhenPayseraOrderInfoIsValid()
        {
            // Arrange
            var payseraOrderInfo = new PayseraOrderInfo
            {
                OrderNumber = 123,
                UserId = 12345,
                Description = "Sample Order",
                PayableAmount = 100.50m,
                PaymentGateway = PaymentGatewayType.Paysera
            };

            // Act
            var result = _payseraMapperLogic.ToMappedOrderDto(payseraOrderInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(payseraOrderInfo.OrderNumber, result.OrderNumber);
            Assert.Equal(payseraOrderInfo.UserId, result.UserId);
            Assert.Equal(payseraOrderInfo.Description, result.Description);
            Assert.Equal(payseraOrderInfo.PayableAmount, result.PayableAmount);
        }
    }
}
