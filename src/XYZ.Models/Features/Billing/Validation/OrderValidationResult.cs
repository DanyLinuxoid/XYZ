namespace XYZ.Models.Features.Billing.Validation
{
    public class OrderValidationResult
    {
        public Dictionary<string, string> ValidationErrors { get; set; } = new Dictionary<string, string>();
    }
}
