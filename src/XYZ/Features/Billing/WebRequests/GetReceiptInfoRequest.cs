using System.ComponentModel.DataAnnotations;

namespace XYZ.Web.Features.Billing.WebRequests
{
    public class GetReceiptInfoRequest
    {
        [Required(ErrorMessage = "ReceiptId is required.")]
        [MinLength(1, ErrorMessage = "ReceiptId cannot be empty.")]
        public string ReceiptId { get; set; }
    }
}
