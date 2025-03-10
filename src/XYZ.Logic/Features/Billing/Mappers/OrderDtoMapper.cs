using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Mappers
{
    public static class OrderDtoMapper
    {
        public static ORDER ToDbo(this OrderDto dto)
        {
            return new ORDER()
            {
                PAYABLE_AMOUNT = dto.PayableAmount,
                DESCRIPTION = dto.Description,
                ORDER_NUMBER = dto.OrderNumber,
                USER_ID = dto.UserId,
                ORDER_STATUS = (int)dto.OrderStatus,
                PAYPAL_ORDER_ID = dto.PaypalOrderId,
                PAYSERA_ORDER_ID = dto.PayseraOrderId,
            };
        }

        public static OrderDto ToDto(this ORDER dbo)
        {
            return new OrderDto()
            {
                PayableAmount = dbo.PAYABLE_AMOUNT,
                Description = dbo.DESCRIPTION,
                OrderNumber = dbo.ORDER_NUMBER,
                UserId = dbo.USER_ID,
                OrderStatus = (OrderStatus)dbo.ORDER_STATUS,
                PaypalOrderId = dbo.PAYPAL_ORDER_ID,
                PayseraOrderId = dbo.PAYSERA_ORDER_ID,
            };
        }
    }
}