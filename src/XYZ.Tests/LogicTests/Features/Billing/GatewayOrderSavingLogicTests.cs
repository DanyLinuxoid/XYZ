using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYSERA_ORDER_TABLE;
using XYZ.Logic.Features.Billing.Common;

namespace XYZ.Tests.Logic.Features.Billing.Paysera
{
    public class GatewayOrderSavingLogicTests
    {
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly Mock<ICommandRepository<PAYSERA_GATEWAY_ORDER>> _commandRepositoryMock;
        private readonly GatewayOrderSavingLogic<PAYSERA_GATEWAY_ORDER> _payseraGatewayOrderSavingLogic;

        public GatewayOrderSavingLogicTests()
        {
            _databaseLogicMock = new Mock<IDatabaseLogic>();
            _commandRepositoryMock = new Mock<ICommandRepository<PAYSERA_GATEWAY_ORDER>>();
            _payseraGatewayOrderSavingLogic = new GatewayOrderSavingLogic<PAYSERA_GATEWAY_ORDER>(
                _databaseLogicMock.Object,
                _commandRepositoryMock.Object
            );
        }

        [Fact]
        public async Task SaveOrder_ReturnsOrderId_WhenOrderIsSavedSuccessfully()
        {
            // Arrange
            var expectedOrderId = 123L;
            var order = new PAYSERA_GATEWAY_ORDER();

            _databaseLogicMock
                .Setup(db => db.CommandAsync(_commandRepositoryMock.Object, CommandTypes.Create, order))
                .ReturnsAsync(expectedOrderId);

            // Act
            var result = await _payseraGatewayOrderSavingLogic.SaveOrder(order);

            // Assert
            Assert.Equal(expectedOrderId, result);
            _databaseLogicMock.Verify(
                db => db.CommandAsync(_commandRepositoryMock.Object, CommandTypes.Create, order),
                Times.Once
            );
        }
    }
}