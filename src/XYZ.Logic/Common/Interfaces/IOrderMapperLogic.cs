using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Mapping logic for dto<->dbo.
    /// </summary>
    public interface IOrderMapperLogic<T>
    {
        /// <summary>
        /// Maps generic order info to specific order.
        /// </summary>
        /// <param name="order">Generic order info.</param>
        /// <returns>Order info.</returns>
        public T ToMappedOrderInfo(OrderInfo order);

        /// <summary>
        /// Maps order info to dto which later can be saved to db (after dbo map).
        /// </summary>
        /// <param name="order">Specific order info.</param>
        /// <returns>Order generic dto.</returns>
        public OrderDto ToMappedOrderDto(T order);
    }
}
