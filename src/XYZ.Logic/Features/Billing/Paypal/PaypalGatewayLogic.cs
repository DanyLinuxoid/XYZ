using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Base;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;

namespace XYZ.Logic.Features.Billing.Paypal
{
    /// <summary>
    /// Gateway specific logic.
    /// </summary>
    public class PaypalGatewayLogic : GatewayLogicBase<PaypalOrderInfo, PaypalOrderResult>, IPaymentGatewayLogic
    {
        /// <summary>
        /// Main order logic.
        /// </summary>
        private readonly IOrderLogic _orderLogic;

        /// <summary>
        /// Specific gateway type
        /// </summary>
        public override PaymentGatewayType GatewayType => PaymentGatewayType.PayPal;

        /// <summary>
        /// Gateway specific constructor.
        /// </summary>
        /// <param name="paypalApiLogic">Paypal direct API logic.</param>
        /// <param name="paypalMapperLogic">Paypal objects mapper.</param>
        /// <param name="exceptionSaverLogic">Exception saving.</param>
        /// <param name="simpleLogger">Logger to be used in base.</param>
        /// <param name="orderLogic">Main order logic.</param>
        /// <param name="databaseLogic">Database access.</param>
        public PaypalGatewayLogic(
            ISimpleLogger logger,
            IApiOrderLogic<PaypalOrderInfo, PaypalOrderResult> paypalApiLogic,
            IOrderMapperLogic<PaypalOrderInfo> paypalMapperLogic,
            IExceptionSaverLogic exceptionSaverLogic,
            ISimpleLogger simpleLogger,
            IOrderLogic orderLogic,
            IDatabaseLogic databaseLogic) : base(simpleLogger, paypalApiLogic, exceptionSaverLogic, paypalMapperLogic, databaseLogic)
        {
            _orderLogic = orderLogic;
        }

        /// <summary>
        /// Main gateway public access entrypoint.
        /// </summary>
        /// <param name="order">Generic order from logic.</param>
        /// <returns>Order execution result with details.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if null parameters.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when order cannot be processed.</exception>
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
                throw new InvalidOperationException($"Order with number {order.OrderNumber} is not bound with {GatewayType}");
            else if (orderFull?.ORDER_STATUS == (int)OrderStatus.Completed) // We disallow gateway switching on established orders
                throw new InvalidOperationException($"Order with number {order.OrderNumber} is in status {OrderStatus.Completed}.");

            PaypalOrderResult result = await GetProcessResultAsync(mappedOrder);

            var mappedDto = _mapper.ToMappedOrderDto(mappedOrder);
            mappedDto.OrderStatus = result.OrderStatus;
            mappedDto.PaypalOrderId = orderFull == null 
                ? await _databaseLogic.CommandAsync(new PAYPAL_GATEWAY_ORDER_CUD(), CommandTypes.Create, new PAYPAL_GATEWAY_ORDER()) 
                : orderFull.PAYPAL_ORDER_ID;
            if (orderFull == null) // If new order and didn't fail earlier
                await _orderLogic.SaveOrder(mappedDto);
            else
                await _orderLogic.UpdateOrder(mappedDto);
            return result;
        }
    }
}