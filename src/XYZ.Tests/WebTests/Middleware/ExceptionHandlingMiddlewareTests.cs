using Microsoft.AspNetCore.Http;

using Moq;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Common.Middleware;

namespace XYZ.Tests.WebTests.Common.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<IExceptionSaverLogic> _exceptionSaverLogicMock;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddlewareTests()
        {
            _exceptionSaverLogicMock = new Mock<IExceptionSaverLogic>();
            _next = (HttpContext context) => Task.CompletedTask;
        }

        [Fact]
        public async Task InvokeAsync_WhenNoException_ReturnsSuccess()
        {
            // Arrange
            var middleware = new ExceptionHandlingMiddleware(_next, _exceptionSaverLogicMock.Object);

            var context = new DefaultHttpContext();
            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Act
                await middleware.InvokeAsync(context);

                // Assert
                Assert.Equal(200, context.Response.StatusCode); // If no exception, no status change
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = new StreamReader(context.Response.Body).ReadToEnd();
                Assert.DoesNotContain("message", responseText); // No error message
            }
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var middleware = new ExceptionHandlingMiddleware(
                (HttpContext context) =>
                {
                    throw new Exception("Test exception");
                },
                _exceptionSaverLogicMock.Object);

            var context = new DefaultHttpContext();
            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Act
                await middleware.InvokeAsync(context);

                // Assert
                Assert.Equal(500, context.Response.StatusCode); // Status should be 500 for internal error
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = new StreamReader(context.Response.Body).ReadToEnd();
                Assert.Contains("Something went wrong", responseText); // Ensure the error message is present
                Assert.Contains("code", responseText); // Ensure the error code is included
            }
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_CallsExceptionSaverLogic()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var middleware = new ExceptionHandlingMiddleware(
                (HttpContext context) => throw exception,
                _exceptionSaverLogicMock.Object);

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _exceptionSaverLogicMock.Verify(x => x.SaveUnhandledErrorAsync(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_HandlesLoggingFallback()
        {
            // Arrange
            _exceptionSaverLogicMock
                .Setup(x => x.SaveUnhandledErrorAsync(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("DB save failed"));

            var exception = new Exception("Test exception");

            var middleware = new ExceptionHandlingMiddleware(
                (HttpContext context) => throw exception,
                _exceptionSaverLogicMock.Object);

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _exceptionSaverLogicMock.Verify(x => x.SaveUnhandledErrorAsync(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(2));
        }
    }
}
