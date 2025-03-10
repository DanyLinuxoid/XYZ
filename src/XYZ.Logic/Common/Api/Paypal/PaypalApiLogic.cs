using Newtonsoft.Json;

using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Common.Api.Paypal
{
    /// <summary>
    /// Main logic to access Paypal API directly.
    /// </summary>
    public class PaypalApiLogic : IApiOrderLogic<PaypalOrderInfo, PaypalOrderResult>
    {
        /// <summary>
        /// Process Paypal order and gets result.
        /// </summary>
        /// <param name="paypalOrderInfo">Paypal specific order.</param>
        /// <returns>Paypal order processing result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
        public async Task<PaypalOrderResult> GetProcessedOrderResultAsync(PaypalOrderInfo paypalOrderInfo)
        {
            await Task.Delay(100); // Http delay +/-
            var httpResponse = GetHttpProcessingTestResult();
            var jsonResult = JsonConvert.DeserializeObject<PaypalProcessingOrderResponseResult>(httpResponse);
            if (jsonResult == null)
                throw new ArgumentNullException($"{nameof(PaypalApiLogic)}-{nameof(GetProcessedOrderResultAsync)} Json result is null");

            return new PaypalOrderResult()
            {
                IsSuccess = jsonResult.Result == PaypalResponseResult.Success,
                GatewayTransactionId = jsonResult.PayPalTransactionId,
                OrderStatus = jsonResult.Result == PaypalResponseResult.Success ? OrderStatus.Completed : OrderStatus.Failed,
            };
        }

        /// <summary>
        /// Testing method that stores testing data.
        /// </summary>
        /// <returns>Simulated http result.</returns>
        private string GetHttpProcessingTestResult() =>
            @"
            {
                ""payPalTransactionId"": """ + Guid.NewGuid().ToString() + @""",
                ""result"": ""Success""
            }";
    }
}
