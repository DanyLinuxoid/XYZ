using Moq;

using XYZ.Logic.Billing.Factory;
using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Enums;

namespace XYZ.Tests.Logic.Billing.Factory
{
    public class BillingGatewayFactoryTests
    {
        [Fact]
        public void GetGatewayLogic_ValidPayPalGatewayType_ReturnsPaypalLogic()
        {
            // Arrange
            var mockPaypalLogic = new Mock<IPaymentGatewayLogic>();
            var mockPayseraLogic = new Mock<IPaymentGatewayLogic>();
            mockPaypalLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.PayPal);
            mockPayseraLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.Paysera);

            var factory = new BillingGatewayFactory(new List<IPaymentGatewayLogic>() { mockPaypalLogic.Object, mockPayseraLogic.Object });

            // Act
            var result = factory.GetPaymentGateway(PaymentGatewayType.PayPal);

            // Assert
            Assert.Equal(mockPaypalLogic.Object, result);
        }

        [Fact]
        public void GetGatewayLogic_ValidPayseraGatewayType_ReturnsPayseraLogic()
        {
            // Arrange
            var mockPaypalLogic = new Mock<IPaymentGatewayLogic>();
            var mockPayseraLogic = new Mock<IPaymentGatewayLogic>();
            mockPaypalLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.PayPal);
            mockPayseraLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.Paysera);

            var factory = new BillingGatewayFactory(new List<IPaymentGatewayLogic>() { mockPaypalLogic.Object, mockPayseraLogic.Object });

            // Act
            var result = factory.GetPaymentGateway(PaymentGatewayType.Paysera);

            // Assert
            Assert.Equal(mockPayseraLogic.Object, result);
        }

        [Fact]
        public void GetGatewayLogic_UnknownGatewayType_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var mockPaypalLogic = new Mock<IPaymentGatewayLogic>();
            var mockPayseraLogic = new Mock<IPaymentGatewayLogic>();
            mockPaypalLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.PayPal);
            mockPayseraLogic.Setup(x => x.GatewayType).Returns(PaymentGatewayType.Paysera);

            var factory = new BillingGatewayFactory(new List<IPaymentGatewayLogic>() { mockPaypalLogic.Object, mockPayseraLogic.Object });

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => factory.GetPaymentGateway((PaymentGatewayType)999));
        }
    }
}
