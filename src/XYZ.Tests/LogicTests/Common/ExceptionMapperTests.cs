using XYZ.Logic.Common.Mappers;

namespace XYZ.Tests.Logic.Common.Mappers
{
    public class ExceptionMapperTests
    {
        [Fact]
        public void ToAppErrorDbo_ReturnsCorrectAppErrorObject()
        {
            // Arrange
            var exception = new Exception("Test exception message");

            // Act
            var result = exception.ToAppErrorDbo();

            // Assert
            Assert.Equal(exception.Message, result.ERROR_MESSAGE);
            Assert.Equal(exception.StackTrace ?? string.Empty, result.STACKTRACE);
        }

        [Fact]
        public void ToUserErrorDbo_ReturnsCorrectUserErrorObject()
        {
            // Arrange
            var exception = new Exception("Test exception message");
            long userId = 12345;

            // Act
            var result = exception.ToUserErrorDbo(userId);

            // Assert
            Assert.Equal(exception.Message, result.ERROR_MESSAGE);
            Assert.Equal(exception.StackTrace ?? string.Empty, result.STACKTRACE);
            Assert.Equal(userId, result.USER_ID);
        }
    }
}
