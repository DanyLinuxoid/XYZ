using Moq;

using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.System.Logger;

namespace XYZ.Tests.Logic.System.Logger
{
    public class SimpleLoggerTests
    {
        private readonly Mock<ITextLogWriter> _textLogWriterMock;
        private readonly SimpleLogger _simpleLogger;

        public SimpleLoggerTests()
        {
            _textLogWriterMock = new Mock<ITextLogWriter>();
            _simpleLogger = new SimpleLogger(_textLogWriterMock.Object);
        }

        [Fact]
        public void Log_CallsLogWrite_WithDefaultLogName_WhenNoLogNameProvided()
        {
            // Arrange
            var text = "Test log message";

            // Act
            _simpleLogger.Log(text);

            // Assert
            _textLogWriterMock.Verify(
                writer => writer.LogWrite(text, "all-logs", It.IsAny<DateTime>()),
                Times.Once);
        }

        [Fact]
        public void Log_CallsLogWrite_WithProvidedLogName_WhenLogNameProvided()
        {
            // Arrange
            var text = "Test log message";
            var customLogName = "custom-log-name";

            // Act
            _simpleLogger.Log(text, customLogName);

            // Assert
            _textLogWriterMock.Verify(
                writer => writer.LogWrite(text, customLogName, It.IsAny<DateTime>()),
                Times.Once);
        }

        [Fact]
        public void Log_DoesNotThrowException_WhenLogWriteThrowsException()
        {
            // Arrange
            var text = "Test log message";
            _textLogWriterMock.Setup(writer => writer.LogWrite(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Log write failed"));

            // Act & Assert
            var exception = Record.Exception(() => _simpleLogger.Log(text));

            // Assert
            Assert.Null(exception); 
        }
    }
}
