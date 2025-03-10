namespace XYZ.Models.Common.Enums
{
    /// <summary>
    /// Payment processing stages.
    /// </summary>
    public enum PaymentProcessingEvent
    {
        PaymentStart,
        ValidationError,
        PaymentFinish,
        PaymentFailed,
        UnhandledError
    }
}
