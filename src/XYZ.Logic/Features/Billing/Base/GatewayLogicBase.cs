using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Logic.Features.Billing.Base
{
    /// <summary>
    /// Shared gateway logic accross all gateways, contains general logic.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class GatewayLogicBase<TInput, TOutput>
        where TInput : OrderInfo
        where TOutput : OrderResult, new()
    {
        /// <summary>
        /// General order mapping logic.
        /// </summary>
        protected readonly IOrderMapperLogic<TInput> _mapper;

        /// <summary>
        /// File logging logic.
        /// </summary>
        private readonly ISimpleLogger _simpleLogger;

        /// <summary>
        /// Gateway general api logic.
        /// </summary>
        private readonly IApiOrderLogic<TInput, TOutput> _apiOrderLogic;

        /// <summary>
        /// Exception saving logic.
        /// </summary>
        private readonly IExceptionSaverLogic _exceptionSaverLogic;

        /// <summary>
        /// Shared gateway logic accross all gateways, contains general logic.
        /// </summary>
        /// <param name="simpleLogger">File logging logic.</param>
        /// <param name="apiOrderLogic">Gateway general api logic.</param>
        /// <param name="exceptionSaverLogic">Exception saving logic.</param>
        /// <param name="mapper">General order mapping logic.</param>
        public GatewayLogicBase(
            ISimpleLogger simpleLogger, 
            IApiOrderLogic<TInput, TOutput> apiOrderLogic, 
            IExceptionSaverLogic exceptionSaverLogic,
            IOrderMapperLogic<TInput> mapper)
        {
            _simpleLogger = simpleLogger;
            _apiOrderLogic = apiOrderLogic;
            _exceptionSaverLogic = exceptionSaverLogic;
            _mapper = mapper;
        }

        /// <summary>
        /// Specific gateway type
        /// </summary>
        public abstract PaymentGatewayType GatewayType { get; }

        /// <summary>
        /// General order validation logic,
        /// </summary>
        /// <param name="order">Specific gateway order.</param>
        /// <returns>Order result.</returns>
        protected virtual TOutput ValidateOrder(TInput order)
        {
            var validationResult = GetOrderValidationResult(order);
            if (validationResult.ValidationErrors.Any())
            {
                _simpleLogger.Log($"{PaymentProcessingEvent.ValidationError} | {GatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
                return new()
                {
                    OrderValidationResult = validationResult,
                };
            }

            return new();
        }

        /// <summary>
        /// Main processing method.
        /// </summary>
        /// <param name="order">Specific order to process.</param>
        /// <returns>Specific order processing result.</returns>
        protected virtual async Task<TOutput> GetProcessResultAsync(TInput order)
        {
            TOutput outputResult;
            try
            {
                outputResult = await GetProcessedOrderResultAsync(order);
                return outputResult;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Order processing failed for user {order.UserId} with order id {order.OrderNumber}";
                await _exceptionSaverLogic.SaveUserErrorAsync(ex, order.UserId);
                _simpleLogger.Log($"({nameof(PaypalGatewayLogic)}): {errorMessage}");
                return new()
                {
                    ProblemDetails = new ProblemDetailed(ex.Message, errorMessage, isException: true),
                    OrderStatus = OrderStatus.Error,
                };
            }
        }

        /// <summary>
        /// Shared validation logic for inputs.
        /// </summary>
        /// <param name="order">Order to validate.</param>
        /// <returns>Order validation result.</returns>
        protected virtual OrderValidationResult GetOrderValidationResult(OrderInfo order)
        {
            var result = new OrderValidationResult();
            if (order.OrderNumber <= 0)
                result.ValidationErrors[nameof(order.OrderNumber)] = "Order number must be more than 0";
            if (order.PayableAmount <= 0)
                result.ValidationErrors[nameof(order.PayableAmount)] = "Payable amount must be more than 0";
            if (order.UserId <= 0)
                result.ValidationErrors[nameof(order.UserId)] = "User id must be more than 0";

            return result;
        }

        /// <summary>
        /// Wrapper for processing which logs results and calls specific API.
        /// </summary>
        /// <param name="order">Specific order that API should process.</param>
        /// <returns>API processing result.</returns>
        protected virtual async Task<TOutput> GetProcessedOrderResultAsync(TInput order) 
        {
            _simpleLogger.Log($"{PaymentProcessingEvent.PaymentStart} | {GatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
            var processResult = await _apiOrderLogic.GetProcessedOrderResultAsync(order);
            PaymentProcessingEvent eventResult = processResult.IsSuccess && !string.IsNullOrEmpty(processResult.GatewayTransactionId) ? PaymentProcessingEvent.PaymentFinish : PaymentProcessingEvent.PaymentFailed;
            _simpleLogger.Log($"{eventResult} | {GatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
            return processResult;
        }
    }
}
