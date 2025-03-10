namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Wrapper for text logger.
    /// </summary>
    public interface ISimpleLogger
    {
        /// <summary>
        /// Logging method.
        /// </summary>
        /// <param name="text">Text to log.</param>
        /// <param name="logName">Log name (just name).</param>
        void Log(string text, string logName = "");
    }
}