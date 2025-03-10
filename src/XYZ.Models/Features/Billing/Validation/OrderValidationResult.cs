namespace XYZ.Models.Features.Billing.Validation
{
    /// <summary>
    /// Validation results class.
    /// </summary>
    public class OrderValidationResult
    {
        /// <summary>
        /// Validation results container with field->error mapping.
        /// </summary>
        public Dictionary<string, string> ValidationErrors { get; set; } = new Dictionary<string, string>();
    }
}
