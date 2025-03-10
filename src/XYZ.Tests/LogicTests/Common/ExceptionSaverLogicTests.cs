using Moq;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
using XYZ.DataAccess.Tables.Base;
using XYZ.DataAccess.Tables.USER_ERROR_TABLE;
using XYZ.Logic.Common.ExceptionSaving;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Common.Mappers;

namespace XYZ.Tests.Logic.Common.ExceptionSaving
{
    public class ExceptionSaverLogicTests
    {
        private readonly Mock<IDatabaseLogic> _databaseLogicMock;
        private readonly Mock<ISimpleLogger> _simpleLoggerMock;
        private readonly ExceptionSaverLogic _exceptionSaverLogic;

        public ExceptionSaverLogicTests()
        {
            _databaseLogicMock = new Mock<IDatabaseLogic>();
            _simpleLoggerMock = new Mock<ISimpleLogger>();
            _exceptionSaverLogic = new ExceptionSaverLogic(_databaseLogicMock.Object, _simpleLoggerMock.Object);
        }

        [Fact]
        public async Task SaveUnhandledErrorAsync_SavesErrorInDatabase_WhenShouldSaveInDbIsTrue()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var additionalText = "Additional info";
            var exceptionModel = exception.ToAppErrorDbo();

            // Act
            await _exceptionSaverLogic.SaveUnhandledErrorAsync(exception, additionalText, shouldSaveInDb: true);

            // Assert
            _simpleLoggerMock.Verify(x => x.Log(It.IsAny<string>(), "unhandled-exceptions-log"), Times.Once);
        }

        [Fact]
        public async Task SaveUnhandledErrorAsync_DoesNotSaveErrorInDatabase_WhenShouldSaveInDbIsFalse()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var additionalText = "Additional info";

            // Act
            await _exceptionSaverLogic.SaveUnhandledErrorAsync(exception, additionalText, false);

            // Assert
            _databaseLogicMock.Verify(x => x.CommandAsync(It.IsAny<ICommandRepository<APP_ERROR_CUD>>(), CommandTypes.Create, It.IsAny<APP_ERROR_CUD>()), Times.Never);
            _simpleLoggerMock.Verify(x => x.Log(It.IsAny<string>(), "unhandled-exceptions-log"), Times.Once);
        }

        [Fact]
        public async Task SaveUserErrorAsync_SavesErrorInDatabase_WhenShouldSaveInDbIsTrue()
        {
            // Arrange
            var exception = new Exception("Test exception");
            long userId = 12345;
            var additionalText = "User-specific info";
            var exceptionModel = exception.ToUserErrorDbo(userId);

            // Act
            await _exceptionSaverLogic.SaveUserErrorAsync(exception, userId, additionalText, true);

            // Assert
            _simpleLoggerMock.Verify(x => x.Log(It.IsAny<string>(), "user-exceptions-log"), Times.Once);
        }

        [Fact]
        public async Task SaveUserErrorAsync_DoesNotSaveErrorInDatabase_WhenShouldSaveInDbIsFalse()
        {
            // Arrange
            var exception = new Exception("Test exception");
            long userId = 12345;
            var additionalText = "User-specific info";

            // Act
            await _exceptionSaverLogic.SaveUserErrorAsync(exception, userId, additionalText, false);

            // Assert
            _databaseLogicMock.Verify(x => x.CommandAsync(It.IsAny<ICommandRepository<USER_ERROR_CUD>>(), CommandTypes.Create, It.IsAny<USER_ERROR_CUD>()), Times.Never);
            _simpleLoggerMock.Verify(x => x.Log(It.IsAny<string>(), "user-exceptions-log"), Times.Once);
        }
    }
}
