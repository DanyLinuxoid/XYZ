using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.WebResults;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IBillingLogic
    {
        Task<OrderProcessingResult> ProcessOrderAsync(OrderInfo order);

        Task<OrderDto?> GetOrderInfoAsync(long userId, long orderId);

        Task<ReceiptDto?> GetReceiptAsync(string receiptId);
    }
}
