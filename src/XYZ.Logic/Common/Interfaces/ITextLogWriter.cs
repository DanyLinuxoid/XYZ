namespace XYZ.Logic.Common.Interfaces
{
    public interface ITextLogWriter
    {
        string GetLogInfo(string logMessage, DateTime dateTime);
        void LogWrite(string text, string fileName, DateTime dateTime);
    }
}