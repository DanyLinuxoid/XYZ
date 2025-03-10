namespace XYZ.Logic.Common.Interfaces
{
    public interface IExceptionSaverLogic
    {
        Task SaveUnhandledErrorAsync(Exception exception, string? additionalText = null, bool shouldSaveInDb = true);
        Task SaveUserErrorAsync(Exception exception, long userId, string? additionalText = null, bool shouldSaveInDb = true);
    }
}