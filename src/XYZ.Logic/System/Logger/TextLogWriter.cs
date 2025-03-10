using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.Logger
{
    /// <summary>
    /// Main low level logger.
    /// This logger version uses locking mechanism which can cause problems on bigger environments with higher throughput.
    /// This logger is used here only for showcasing/pet purposes and should not be used in normal applications.
    /// </summary>
    public class TextLogWriter : ITextLogWriter
    {
        /// <summary>
        /// File locking mechanism.
        /// </summary>
        private readonly object _fileLock = new object();

        /// <summary>
        /// Root folder.
        /// </summary>
        private readonly string _logRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

        /// <summary>
        /// Main method to use for writing logs.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="fileName">File name for logs.</param>
        /// <param name="dateTime">Log date.</param>
        public void LogWrite(string text, string fileName, DateTime dateTime)
        {
            Directory.CreateDirectory(_logRootPath);
            lock (_fileLock)
            {
                string filePath = Path.Combine(_logRootPath, $"{fileName}.txt");
                using (StreamWriter streamWriter = new StreamWriter(filePath, append: true))
                {
                    streamWriter.WriteLine(GetLogInfo(text, DateTime.Now));
                }
            }
        }

        /// <summary>
        /// Log text/entry beautifier.
        /// </summary>
        /// <param name="logMessage">Log text.</param>
        /// <param name="dateTime">Log entry date.</param>
        /// <returns>Formatted text to write into log.</returns>
        private string GetLogInfo(string logMessage, DateTime dateTime) =>
            $"**************************************************************************************\n" +
            $"{dateTime:dddd, dd MMMM yyyy HH:mm:ss}\n{logMessage.Trim()}";
    }
}