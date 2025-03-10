using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Data.Order.Order;
using XYZ.Models.Features.Billing.WebResults;

namespace XYZ.Logic.Features.Billing
{
    /// <summary>
    /// Billing/order processing main logic.
    /// </summary>
    public class BillingLogic : IBillingLogic
    {
        /// <summary>
        /// Payment gateway factory.
        /// </summary>
        private readonly IBillingGatewayFactory _billingGatewayFactory;

        /// <summary>
        /// User info main logic.
        /// </summary>
        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Order receipt logic.
        /// </summary>
        private readonly IReceiptLogic _receiptLogic;

        /// <summary>
        /// Main order logic.
        /// </summary>
        private readonly IOrderLogic _orderLogic;

        /// <summary>
        /// Billing logic main constructor.
        /// </summary>
        /// <param name="billingGatewayFactory">Payment gateway factory.</param>
        /// <param name="userLogic">User info main logic.</param>
        /// <param name="receiptSaveLogic">Order receipt logic.</param>
        /// <param name="orderLogic">Main order logic.</param>
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

        /// <summary>
        /// Gets receipt by receipt string id.
        /// </summary>
        /// <param name="receiptId">String identifier that was printed by us.</param>
        /// <returns>Receipt if found, null if nothing.</returns>
        public async Task<ReceiptDto?> GetReceiptAsync(string receiptId) => await _receiptLogic.GetReceipt(receiptId);

        /// <summary>
        /// Gets order info associated with user.
        /// </summary>
        /// <param name="userId">User main identifier.</param>
        /// <param name="orderNumber">Order number (not main identifier).</param>
        /// <returns>Order if found, null if nothing.</returns>
        public async Task<OrderDto?> GetOrderInfoAsync(long userId, long orderNumber) => await _orderLogic.GetOrderAsync(userId, orderNumber);

        /// <summary>
        /// Main logic to process user payment order request.
        /// </summary>
        /// <param name="order">User filled order info.</param>
        /// <returns>Printed receipt if everything is ok, otherwise validation errors or problem details,</returns>
        /// <exception cref="ArgumentNullException">If parameters passed were null.</exception>
        public async Task<OrderProcessingResult> ProcessOrderAsync(OrderInfo order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderProcessingResult result = new OrderProcessingResult();
            var user = await _userLogic.GetUserInfo(order.UserId);
            if (user == null)
                return new OrderProcessingResult()
                {
                    ProblemDetails = new ProblemDetailed("User not found", $"User with Id {order.UserId} not found"),
                };

            var gatewayLogic = _billingGatewayFactory.GetPaymentGateway(order.PaymentGateway);

            // Main call
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
