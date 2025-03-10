using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE;
using XYZ.Logic.Features.Billing.Paypal;

namespace XYZ.Tests.Logic.Features.Billing.Paypal
{
    public class PaypalGatewayOrderSavingLogicTests
    {
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly PaypalGatewayOrderSavingLogic _paypalGatewayOrderSavingLogic;

        public PaypalGatewayOrderSavingLogicTests()
        {
            _databaseLogicMock = new Mock<IDatabaseLogic>();
            _paypalGatewayOrderSavingLogic = new PaypalGatewayOrderSavingLogic(_databaseLogicMock.Object);
        }

        [Fact]
        public async Task SaveOrder_CallsCommandAsync_WithCorrectParameters()
        {
            // Arrange
            var expectedResult = 12345L;
            _databaseLogicMock
                .Setup(db => db.CommandAsync(It.IsAny<PAYPAL_GATEWAY_ORDER_CUD>(), CommandTypes.Create, It.IsAny<PAYPAL_GATEWAY_ORDER>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _paypalGatewayOrderSavingLogic.SaveOrder();

            // Assert
            Assert.Equal(expectedResult, result);  
            _databaseLogicMock.Verify(db => db.CommandAsync(It.IsAny<PAYPAL_GATEWAY_ORDER_CUD>(), CommandTypes.Create, It.IsAny<PAYPAL_GATEWAY_ORDER>()), Times.Once);  
        }
    }
}
