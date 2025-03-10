using Moq;

using System.ComponentModel;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Logic;

namespace XYZ.Tests.DatabaseTests.MainLogic
{
    public class DatabaseLogicTests
    {
        private readonly Mock<IDatabaseCommandExecutionLogic> _databaseCommandExecutionLogicMock;
        private readonly Mock<IDatabaseQueryExecutionLogic> _databaseQueryExecutionLogicMock;
        private readonly DatabaseLogic _databaseLogic;

        public DatabaseLogicTests()
        {
            _databaseCommandExecutionLogicMock = new Mock<IDatabaseCommandExecutionLogic>();
            _databaseQueryExecutionLogicMock = new Mock<IDatabaseQueryExecutionLogic>();
            _databaseLogic = new DatabaseLogic(
                _databaseCommandExecutionLogicMock.Object,
                _databaseQueryExecutionLogicMock.Object
            );
        }

        [Fact]
        public async Task QueryAsync_CallsExecuteAsync_OnQuery()
        {
            // Arrange
            var mockQuery = new Mock<IQuery<int>>();
            mockQuery.Setup(q => q.ExecuteAsync(_databaseQueryExecutionLogicMock.Object))
                .ReturnsAsync(1);

            // Act
            var result = await _databaseLogic.QueryAsync(mockQuery.Object);

            // Assert
            Assert.Equal(1, result);
            mockQuery.Verify(q => q.ExecuteAsync(_databaseQueryExecutionLogicMock.Object), Times.Once);
        }

        [Fact]
        public async Task CommandAsync_CreateCommand_CallsCreateAsync()
        {
            // Arrange
            var mockCommand = new Mock<ICommandRepository<object>>();
            var model = new object();
            var commandType = CommandTypes.Create;

            mockCommand.Setup(c => c.CreateAsync(model, _databaseCommandExecutionLogicMock.Object))
                .ReturnsAsync(1L);

            // Act
            var result = await _databaseLogic.CommandAsync(mockCommand.Object, commandType, model);

            // Assert
            Assert.Equal(1L, result);
            mockCommand.Verify(c => c.CreateAsync(model, _databaseCommandExecutionLogicMock.Object), Times.Once);
        }

        [Fact]
        public async Task CommandAsync_UpdateCommand_CallsUpdateAsync()
        {
            // Arrange
            var mockCommand = new Mock<ICommandRepository<object>>();
            var model = new object();
            var commandType = CommandTypes.Update;

            mockCommand.Setup(c => c.UpdateAsync(model, _databaseCommandExecutionLogicMock.Object))
                .ReturnsAsync(1L);

            // Act
            var result = await _databaseLogic.CommandAsync(mockCommand.Object, commandType, model);

            // Assert
            Assert.Equal(1L, result);
            mockCommand.Verify(c => c.UpdateAsync(model, _databaseCommandExecutionLogicMock.Object), Times.Once);
        }

        [Fact]
        public async Task CommandAsync_DeleteCommand_CallsDeleteAsync()
        {
            // Arrange
            var mockCommand = new Mock<ICommandRepository<object>>();
            var model = new object();
            var commandType = CommandTypes.Delete;

            mockCommand.Setup(c => c.DeleteAsync(model, _databaseCommandExecutionLogicMock.Object))
                .ReturnsAsync(1L);

            // Act
            var result = await _databaseLogic.CommandAsync(mockCommand.Object, commandType, model);

            // Assert
            Assert.Equal(1L, result);
            mockCommand.Verify(c => c.DeleteAsync(model, _databaseCommandExecutionLogicMock.Object), Times.Once);
        }

        [Fact]
        public async Task CommandAsync_ThrowsInvalidEnumArgumentException_WhenCommandTypeIsUnknown()
        {
            // Arrange
            var mockCommand = new Mock<ICommandRepository<object>>();
            var model = new object();
            var commandType = (CommandTypes)999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidEnumArgumentException>(() =>
                _databaseLogic.CommandAsync(mockCommand.Object, commandType, model));
            Assert.Equal("999 is unknown type", exception.Message);
        }
    }
}
