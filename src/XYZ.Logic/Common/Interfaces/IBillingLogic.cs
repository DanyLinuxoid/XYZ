using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.WebResults;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Billing/order processing main logic.
    /// </summary>
    public interface IBillingLogic
    {
        /// <summary>
        /// Gets receipt by receipt string id.
        /// </summary>
        /// <param name="receiptId">String identifier that was printed by us.</param>
        /// <returns>Receipt if found, null if nothing.</returns>
        Task<OrderProcessingResult> ProcessOrderAsync(OrderInfo receiptId);

        /// <summary>
        /// Gets order info associated with user.
        /// </summary>
        /// <param name="userId">User main identifier.</param>
        /// <param name="orderNumber">Order number (not main identifier).</param>
        /// <returns>Order if found, null if nothing.</returns>
        Task<OrderDto?> GetOrderInfoAsync(long userId, long orderNumber);

        /// <summary>
        /// Gets receipt by receipt string id.
        /// </summary>
        /// <param name="receiptId">String identifier that was printed by us.</param>
        /// <returns>Receipt if found, null if nothing.</returns>
        Task<ReceiptDto?> GetReceiptAsync(string receiptId);
    }
}
