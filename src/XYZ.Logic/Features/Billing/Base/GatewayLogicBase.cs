using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Logic.Features.Billing.Base
{
    public abstract class GatewayLogicBase<TInput, TOutput>
        where TInput : OrderInfo
        where TOutput : OrderResult, new()
    {
        protected readonly IGatewayOrderSavingLogic _gatewayOrderSavingLogic;
        protected readonly IOrderMapperLogic<TInput> _mapper;

        private readonly ISimpleLogger _simpleLogger;
        private readonly IApiOrderLogic<TInput, TOutput> _apiOrderLogic;
        private readonly IExceptionSaverLogic _exceptionSaverLogic;

        public GatewayLogicBase(
            ISimpleLogger simpleLogger, 
            IApiOrderLogic<TInput, TOutput> apiOrderLogic, 
            IGatewayOrderSavingLogic gatewayOrderSavingLogic, 
            IExceptionSaverLogic exceptionSaverLogic,
            IOrderMapperLogic<TInput> mapper)
        {
            _simpleLogger = simpleLogger;
            _apiOrderLogic = apiOrderLogic;
            _gatewayOrderSavingLogic = gatewayOrderSavingLogic;
            _exceptionSaverLogic = exceptionSaverLogic;
            _mapper = mapper;
        }

        protected abstract GatewayType _gatewayType { get; }

        protected virtual TOutput ValidateOrder(TInput order)
        {
            var validationResult = GetOrderValidationResult(order);
            if (validationResult.ValidationErrors.Any())
            {
                _simpleLogger.Log($"{PaymentProcessingEvent.ValidationError} | {_gatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
                return new()
                {
                    OrderValidationResult = validationResult,
                };
            }

            return new();
        }

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

        protected virtual async Task<TOutput> GetProcessedOrderResultAsync(TInput order) 
        {
            _simpleLogger.Log($"{PaymentProcessingEvent.PaymentStart} | {_gatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
            var processResult = await _apiOrderLogic.GetProcessedOrderResultAsync(order);
            PaymentProcessingEvent eventResult = processResult.IsSuccess && !string.IsNullOrEmpty(processResult.GatewayTransactionId) ? PaymentProcessingEvent.PaymentFinish : PaymentProcessingEvent.PaymentFailed;
            _simpleLogger.Log($"{eventResult} | {_gatewayType} | {nameof(order.UserId)}: {order.UserId} | {nameof(order.OrderNumber)}: {order.OrderNumber}");
            return processResult;
        }
    }
}
