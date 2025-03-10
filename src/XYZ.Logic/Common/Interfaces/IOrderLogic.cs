using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IOrderLogic
    {
        Task<OrderDto?> GetOrderAsync(long userId, long orderNumber);
        Task<long> SaveOrder(OrderDto? billingOrder);
        Task UpdateOrder(OrderDto? billingOrder);
    }
}