using Newtonsoft.Json;

using XYZ.Models.Common.Enums;

namespace XYZ.Models.Common.Api.Paypal
{
    public class PaypalProcessingOrderResponseResult
    {
        [JsonProperty("payPalTransactionId")]
        public string PayPalTransactionId { get; set; }

        [JsonProperty("result")]
        public PaypalResponseResult Result { get; set; }
    }
}
