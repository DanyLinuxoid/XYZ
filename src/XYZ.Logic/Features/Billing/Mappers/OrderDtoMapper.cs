using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Mappers
{
    /// <summary>
    /// Simple dto<->dbo mapping for orders.
    /// </summary>
    public static class OrderDtoMapper
    {
        /// <summary>
        /// Maps dbo order to dto for db saving.
        /// </summary>
        /// <param name="dto">Dto order.</param>
        /// <returns>Dto order.</returns>
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

        /// <summary>
        /// Maps dbo order to dto.
        /// </summary>
        /// <param name="dbo">Dbo receipt.</param>
        /// <returns>Dto receipt.</returns>
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