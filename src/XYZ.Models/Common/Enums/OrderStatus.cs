namespace XYZ.Models.Common.Enums
{
    /// <summary>
    /// User order statuses.
    /// </summary>
    public enum OrderStatus
    {
        Unknown,
        Completed,
        Error,
        UnhandledError,
        ValidationError,
        Processing,
        Failed
    }
}
