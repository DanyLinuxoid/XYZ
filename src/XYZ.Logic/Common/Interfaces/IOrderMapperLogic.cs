using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IOrderMapperLogic<T>
    {
        public T ToMappedOrderInfo(OrderInfo order);

        public OrderDto ToMappedOrderDto(T order);
    }
}
