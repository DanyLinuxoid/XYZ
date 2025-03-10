using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Data.Order.Order;
using XYZ.Models.Features.Billing.WebResults;

namespace XYZ.Logic.Features.Billing
{
    public class BillingLogic : IBillingLogic
    {
        private readonly IBillingGatewayFactory _billingGatewayFactory;
        private readonly IUserLogic _userLogic;
        private readonly IReceiptLogic _receiptLogic;
        private readonly IOrderLogic _orderLogic;

        public BillingLogic(
            IBillingGatewayFactory billingGatewayFactory,
            IUserLogic userLogic,
            IReceiptLogic receiptSaveLogic,
            IOrderLogic orderLogic)
        {
            _billingGatewayFactory = billingGatewayFactory;
            _userLogic = userLogic;
            _receiptLogic = receiptSaveLogic;
            _orderLogic = orderLogic;
        }

        public async Task<ReceiptDto?> GetReceiptAsync(string receiptId) => await _receiptLogic.GetReceipt(receiptId);

        public async Task<OrderDto?> GetOrderInfoAsync(long userId, long orderId) => await _orderLogic.GetOrderAsync(userId, orderId);

        public async Task<OrderProcessingResult> ProcessOrderAsync(OrderInfo order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderProcessingResult result = new OrderProcessingResult();
            var user = await _userLogic.GetUserInfoShort(order.UserId);
            if (user == null)
                return new OrderProcessingResult()
                {
                    ProblemDetails = new ProblemDetailed(OrderProcessingError.UserNotFound.ToString(), $"User with Id {order.UserId} not found"),
                };

            var gatewayLogic = _billingGatewayFactory.GetGatewayLogic(order.PaymentGateway);

            // Critical section
            OrderResult orderResult = await gatewayLogic.GetGatewayOrderProcessResultAsync(order);
            if (orderResult.OrderValidationResult.ValidationErrors.Any())
                return new OrderProcessingResult()
                {
                    OrderValidationResult = orderResult.OrderValidationResult,
                };
            else if (!orderResult.IsSuccess)
                return new OrderProcessingResult()
                {
                    ProblemDetails = orderResult.ProblemDetails,
                };

            // All ok - save receipt
            var receipt = new ReceiptDto
            {
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                GatewayTransactionId = orderResult.GatewayTransactionId,
                ReceiptId = Guid.NewGuid().ToString(),
            };
            await _receiptLogic.SaveReceipt(receipt);
            return new OrderProcessingResult()
            {
                Receipt = receipt,
            };
        }
    }
}
