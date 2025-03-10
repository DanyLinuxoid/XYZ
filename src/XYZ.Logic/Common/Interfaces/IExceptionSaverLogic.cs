namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Exception saving to file/db.
    /// </summary>
    public interface IExceptionSaverLogic
    {
        /// <summary>
        /// Saves unhandled application exception,
        /// </summary>
        /// <param name="exception">Unhandled application exception.</param>
        /// <param name="additionalText">Additional text for exception, can be anything worth mentioning.</param>
        /// <param name="shouldSaveInDb">If should save to database, ot only to file.</param>
        Task SaveUnhandledErrorAsync(Exception exception, string? additionalText = null, bool shouldSaveInDb = true);

        /// <summary>
        /// Saves user exception,
        /// </summary>
        /// <param name="exception">User exception.</param>
        /// <param name="additionalText">Additional text for exception, can be anything worth mentioning.</param>
        /// <param name="shouldSaveInDb">If should save to database, ot only to file.</param>
        Task SaveUserErrorAsync(Exception exception, long userId, string? additionalText = null, bool shouldSaveInDb = true);
    }
}