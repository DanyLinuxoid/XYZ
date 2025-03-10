using System.ComponentModel.DataAnnotations;

namespace XYZ.Web.Features.Billing.WebRequests
{
    /// <summary>
    /// Get receipt associated with order.
    /// </summary>
    public class GetReceiptInfoRequest
    {
        /// <summary>
        /// Receipt to get info for. Comes after finished order.
        /// </summary>
        [Required(ErrorMessage = "ReceiptId is required.")]
        [MinLength(1, ErrorMessage = "ReceiptId cannot be empty.")]
        public string ReceiptId { get; set; }
    }
}
