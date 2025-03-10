using Newtonsoft.Json;

using XYZ.Models.Common.Enums;

namespace XYZ.Models.Common.Api.Paysera
{
    public class PayseraProcessingOrderResult
    {
        [JsonProperty("payseraTransactionId")]
        public string PayseraTransactionId { get; set; }

        [JsonProperty("result")]
        public PaypalResponseResult Result { get; set; }

        [JsonProperty("reason")]
        public string ErrorReason { get; set; }
    }
}
