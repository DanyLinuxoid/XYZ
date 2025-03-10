using Moq;

using XYZ.DataAccess.Interfaces;
using XYZ.Logic.System.SmokeTest;

namespace XYZ.Tests.Logic.System.SmokeTest
{
    public class SmokeTestLogicTests
    {
        private readonly Mock<IDatabaseUtilityLogic> _databaseUtilityLogicMock;
        private readonly SmokeTestLogic _smokeTestLogic;

        public SmokeTestLogicTests()
        {
            _databaseUtilityLogicMock = new Mock<IDatabaseUtilityLogic>();
            _smokeTestLogic = new SmokeTestLogic(_databaseUtilityLogicMock.Object);
        }

        [Fact]
        public async Task IsSystemFunctional_ReturnsTrue_WhenDbIsFunctional()
        {
            // Arrange
            _databaseUtilityLogicMock
                .Setup(x => x.PingAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _smokeTestLogic.IsSystemFunctional();

            // Assert
            Assert.True(result);
            _databaseUtilityLogicMock.Verify(x => x.PingAsync(), Times.Once);
        }

        [Fact]
        public async Task IsSystemFunctional_ReturnsFalse_WhenDbIsNotFunctional()
        {
            // Arrange
            _databaseUtilityLogicMock
                .Setup(x => x.PingAsync())
                .ReturnsAsync(false);

            // Act
            var result = await _smokeTestLogic.IsSystemFunctional();

            // Assert
            Assert.False(result);
            _databaseUtilityLogicMock.Verify(x => x.PingAsync(), Times.Once);
        }
    }
}
