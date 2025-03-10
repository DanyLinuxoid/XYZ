using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYSERA_ORDER_TABLE;
using XYZ.Logic.Features.Billing.Paysera;

namespace XYZ.Tests.Logic.Features.Billing.Paysera
{
    public class PayseraGatewayOrderSavingLogicTests
    {
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly PayseraGatewayOrderSavingLogic _payseraGatewayOrderSavingLogic;

        public PayseraGatewayOrderSavingLogicTests()
        {
            _databaseLogicMock = new Mock<IDatabaseLogic>();
            _payseraGatewayOrderSavingLogic = new PayseraGatewayOrderSavingLogic(_databaseLogicMock.Object);
        }

        [Fact]
        public async Task SaveOrder_ReturnsOrderId_WhenOrderIsSavedSuccessfully()
        {
            // Arrange
            var expectedOrderId = 123L;
            _databaseLogicMock.Setup(x => x.CommandAsync(It.IsAny<PAYSERA_GATEWAY_ORDER_CUD>(), CommandTypes.Create, It.IsAny<PAYSERA_GATEWAY_ORDER>()))
                .ReturnsAsync(expectedOrderId);

            // Act
            var result = await _payseraGatewayOrderSavingLogic.SaveOrder();

            // Assert
            Assert.Equal(expectedOrderId, result);
            _databaseLogicMock.Verify(x => x.CommandAsync(It.IsAny<PAYSERA_GATEWAY_ORDER_CUD>(), CommandTypes.Create, It.IsAny<PAYSERA_GATEWAY_ORDER>()), Times.Once);
        }
    }
}
