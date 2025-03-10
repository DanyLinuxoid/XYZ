using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.Logger
{
    public class SimpleLogger : ISimpleLogger
    {
        private ITextLogWriter _textLogWriter;
        const string _logName = "all-logs";

        public SimpleLogger(ITextLogWriter textLogWriter)
        {
            _textLogWriter = textLogWriter;
        }

        public void Log(string text, string logName = "")
        {
            try
            {
                _textLogWriter.LogWrite(text, string.IsNullOrEmpty(logName) ? _logName : logName, DateTime.Now);
            }
            catch (Exception) { }
        }
    }
}
