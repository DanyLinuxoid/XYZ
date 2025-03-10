using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.Logger
{
    /// <summary>
    /// Wrapper for text logger.
    /// </summary>
    public class SimpleLogger : ISimpleLogger
    {
        /// <summary>
        /// Actual low-level logger.
        /// </summary>
        private ITextLogWriter _textLogWriter;

        /// <summary>
        /// Main log name is used if log name is not provided.
        /// </summary>
        const string _logName = "all-logs";

        /// <summary>
        /// Wrapper constructor.
        /// </summary>
        /// <param name="textLogWriter"></param>
        public SimpleLogger(ITextLogWriter textLogWriter)
        {
            _textLogWriter = textLogWriter;
        }

        /// <summary>
        /// Logging method.
        /// </summary>
        /// <param name="text">Text to log.</param>
        /// <param name="logName">Log name (just name).</param>
        public void Log(string text, string logName = "") => _textLogWriter.LogWrite(text, string.IsNullOrEmpty(logName) ? _logName : logName, DateTime.Now);
    }
}
