namespace XYZ.Models.Common.Enums
{
    public enum PaymentProcessingEvent
    {
        PaymentStart,
        ValidationError,
        PaymentFinish,
        PaymentFailed,
        UnhandledError
    }
}
