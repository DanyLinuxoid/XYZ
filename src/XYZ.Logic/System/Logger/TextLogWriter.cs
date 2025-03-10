using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.Logger
{
    public class TextLogWriter : ITextLogWriter
    {
        private readonly object _fileLock = new object();

        private readonly string _logRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

        public void LogWrite(string text, string fileName, DateTime dateTime)
        {
            Directory.CreateDirectory(_logRootPath);
            lock (_fileLock)
            {
                string filePath = Path.Combine(_logRootPath, $"{fileName}.txt");
                using (StreamWriter streamWriter = new StreamWriter(filePath, append: true))
                {
                    LogExact(GetLogInfo(text, DateTime.Now), streamWriter);
                }
            }
        }

        public string GetLogInfo(string logMessage, DateTime dateTime) =>
            $"**************************************************************************************\n" +
            $"{dateTime:dddd, dd MMMM yyyy HH:mm:ss}\n{logMessage.Trim()}";

        private void LogExact(string logMessage, TextWriter txtWriter) =>
            txtWriter.WriteLine(logMessage);
    }
}