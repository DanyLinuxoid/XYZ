using Newtonsoft.Json;

using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;

namespace XYZ.Logic.Common.Api.Paysera
{
    /// <summary>
    /// Main logic to access Paysera API directly.
    /// </summary>
    public class PayseraApiLogic : IApiOrderLogic<PayseraOrderInfo, PayseraOrderResult>
    {
        /// <summary>
        /// Process Paysera order and gets result.
        /// </summary>
        /// <param name="paypalOrderInfo">Paysera specific order.</param>
        /// <returns>Paysera order processing result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
        public async Task<PayseraOrderResult> GetProcessedOrderResultAsync(PayseraOrderInfo paypalOrderInfo)
        {
            await Task.Delay(100); // Http delay +/-
            var httpResponse = GetHttpProcessingTestResult();
            var jsonResult = JsonConvert.DeserializeObject<PayseraProcessingOrderResult>(httpResponse);
            if (jsonResult == null)
                throw new ArgumentNullException($"{nameof(PayseraApiLogic)}-{nameof(GetProcessedOrderResultAsync)} Json result is null");

            bool isSuccess = jsonResult.Result == PaypalResponseResult.Success && string.IsNullOrEmpty(jsonResult.ErrorReason);
            return new PayseraOrderResult()
            {
                IsSuccess = isSuccess,
                GatewayTransactionId = jsonResult.PayseraTransactionId,
                ProblemDetails = isSuccess ? null : new ProblemDetailed($"{PaymentGatewayType.Paysera} Api failed to process order {paypalOrderInfo.OrderNumber}", jsonResult.ErrorReason),
                OrderStatus = isSuccess ? OrderStatus.Completed : OrderStatus.Failed
            };
        }

        /// <summary>
        /// Testing method that stores testing data.
        /// </summary>
        /// <returns>Simulated http result.</returns>
        private string GetHttpProcessingTestResult() =>
            @"
            {
                ""payseraTransactionId"": ""null"",
                ""result"": ""Failed"",
                ""reason"": ""Technical maintenance is currently underway"" 
            }";
    }
}
