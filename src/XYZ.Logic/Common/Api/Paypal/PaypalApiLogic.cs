using Newtonsoft.Json;

using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Common.Api.Paypal
{
    public class PaypalApiLogic : IPaypalApiLogic<PaypalOrderInfo, PaypalOrderResult>
    {
        private const GatewayType _gatewayType = GatewayType.PayPal;

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

        private string GetHttpProcessingTestResult() =>
            @"
            {
                ""payPalTransactionId"": """ + Guid.NewGuid().ToString() + @""",
                ""result"": ""Success""
            }";
    }
}
