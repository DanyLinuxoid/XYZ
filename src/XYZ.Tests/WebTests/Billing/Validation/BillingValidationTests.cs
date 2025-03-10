using System.ComponentModel.DataAnnotations;

using XYZ.Models.Common.Enums;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Tests.Validation
{
    public class BillingValidationTests
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Fact]
        public void GetOrderInfoRequest_WhenValid_IsValid()
        {
            // Arrange
            var request = new GetOrderInfoRequest { UserId = 1, OrderNumber = 100 };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void GetOrderInfoRequest_WhenInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var request = new GetOrderInfoRequest { UserId = 0, OrderNumber = 0 };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Equal(2, results.Count);
            foreach (var item in results)
            {
                Assert.NotNull(item.ErrorMessage);
                Assert.NotEmpty(item.ErrorMessage);
            }
        }

        [Fact]
        public void GetReceiptInfoRequest_WhenValid_IsValid()
        {
            // Arrange
            var request = new GetReceiptInfoRequest { ReceiptId = "ABC123" };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void GetReceiptInfoRequest_WhenEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var request = new GetReceiptInfoRequest { ReceiptId = "" };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Single(results);
            foreach (var item in results)
            {
                Assert.NotNull(item.ErrorMessage);
                Assert.NotEmpty(item.ErrorMessage);
            }
        }

        [Fact]
        public void BillingOrderProcessingRequest_WhenValid_IsValid()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest
            {
                PayableAmount = 10.5m,
                OrderNumber = 123,
                UserId = 456,
                PaymentGateway = GatewayType.PayPal,
                Description = "Test Payment"
            };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void BillingOrderProcessingRequest_WhenInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var request = new BillingOrderProcessingRequest
            {
                PayableAmount = 0,
                OrderNumber = 0,
                UserId = 0,
                PaymentGateway = 0,
                Description = "Invalid"
            };

            // Act
            var isValid = ValidateModel(request, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Equal(4, results.Count);
            foreach (var item in results)
            {
                Assert.NotNull(item.ErrorMessage);
                Assert.NotEmpty(item.ErrorMessage);
            }
        }
    }
}
