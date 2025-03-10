namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Main low level logger.
    /// This logger version uses locking mechanism which can cause problems on bigger environments with higher throughput.
    /// This logger is used here only for showcasing/pet purposes and should not be used in normal applications.
    /// </summary>
    public interface ITextLogWriter
    {
        /// <summary>
        /// Main method to use for writing logs.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="fileName">File name for logs.</param>
        /// <param name="dateTime">Log date.</param>
        void LogWrite(string text, string fileName, DateTime dateTime);
    }
}