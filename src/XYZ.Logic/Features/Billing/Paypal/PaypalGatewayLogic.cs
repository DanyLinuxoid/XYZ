using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Base;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;

namespace XYZ.Logic.Features.Billing.Paypal
{
    public class PaypalGatewayLogic : GatewayLogicBase<PaypalOrderInfo, PaypalOrderResult>, IPaypalGatewayLogic
    {
        private readonly IOrderLogic _orderLogic;
        private readonly IDatabaseLogic _databaseLogic;

        protected override GatewayType _gatewayType { get; } = GatewayType.PayPal;

        public PaypalGatewayLogic(
            ISimpleLogger logger,
            IPaypalApiLogic<PaypalOrderInfo, PaypalOrderResult> paypalApiLogic,
            IOrderMapperLogic<PaypalOrderInfo> paypalMapperLogic,
            IPaypalGatewayOrderSavingLogic paypalOrderSavingLogic,
            IExceptionSaverLogic exceptionSaverLogic,
            ISimpleLogger simpleLogger,
            IOrderLogic orderLogic,
            IDatabaseLogic databaseLogic) : base(simpleLogger, paypalApiLogic, paypalOrderSavingLogic, exceptionSaverLogic, paypalMapperLogic)
        {
            _orderLogic = orderLogic;
            _databaseLogic = databaseLogic;
        }

        public async Task<OrderResult> GetGatewayOrderProcessResultAsync(OrderInfo order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Validation
            var mappedOrder = _mapper.ToMappedOrderInfo(order);
            var validationResult = ValidateOrder(mappedOrder);
            if (validationResult.OrderValidationResult.ValidationErrors.Any())
                return validationResult;

            ORDER? orderFull = await _databaseLogic.QueryAsync(new OrderByOrderNumberAndUserIdGetQuery(order.UserId, order.OrderNumber));
            if (orderFull?.PAYSERA_ORDER_ID != null) // We allow manipulations on existing order only if it's not finished
                throw new InvalidOperationException($"Order with number {order.OrderNumber} is not binded with {_gatewayType}");
            else if (orderFull?.ORDER_STATUS == (int)OrderStatus.Completed) // We disallow gateway switching on established orders
                throw new InvalidOperationException($"Order with number {order.OrderNumber} is in status {OrderStatus.Completed}.");

            PaypalOrderResult result = await GetProcessResultAsync(mappedOrder);

            var mappedDto = _mapper.ToMappedOrderDto(mappedOrder);
            mappedDto.OrderStatus = result.OrderStatus;
            mappedDto.PaypalOrderId = orderFull == null ? await _gatewayOrderSavingLogic.SaveOrder() : orderFull.PAYPAL_ORDER_ID;
            if (orderFull == null) // If new order and didn't fail earlier
                await _orderLogic.SaveOrder(mappedDto);
            else
                await _orderLogic.UpdateOrder(mappedDto);

            return result;
        }
    }
}
