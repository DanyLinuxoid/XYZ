using Newtonsoft.Json;

using XYZ.Models.Common.Enums;

namespace XYZ.Models.Common.Api.Paypal
{
    /// <summary>
    /// Serialization object that comes as response from paypal.
    /// </summary>
    public class PaypalProcessingOrderResponseResult
    {
        /// <summary>
        /// Paypal internal transaction id for out order.
        /// </summary>
        [JsonProperty("payPalTransactionId")]
        public string PayPalTransactionId { get; set; }

        /// <summary>
        /// Paypal processing result.
        /// </summary>
        [JsonProperty("result")]
        public PaypalResponseResult Result { get; set; }
    }
}
