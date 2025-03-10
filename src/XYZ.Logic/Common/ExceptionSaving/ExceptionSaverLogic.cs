using Newtonsoft.Json;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
using XYZ.DataAccess.Tables.USER_ERROR_TABLE;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Common.Mappers;

namespace XYZ.Logic.Common.ExceptionSaving
{
    /// <summary>
    /// Exception saving to file/db.
    /// </summary>
    public class ExceptionSaverLogic : IExceptionSaverLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private readonly IDatabaseLogic _databaseLogic;

        /// <summary>
        /// File logging.
        /// </summary>
        private readonly ISimpleLogger _simpleLogger;

        /// <summary>
        /// Exception saving to file/db.
        /// </summary>
        public ExceptionSaverLogic(IDatabaseLogic databaseLogic, ISimpleLogger simpleLogger)
        {
            _databaseLogic = databaseLogic;
            _simpleLogger = simpleLogger;
        }

        /// <summary>
        /// Saves unhandled application exception,
        /// </summary>
        /// <param name="exception">Unhandled application exception.</param>
        /// <param name="additionalText">Additional text for exception, can be anything worth mentioning.</param>
        /// <param name="shouldSaveInDb">If should save to database, ot only to file.</param>
        public async Task SaveUnhandledErrorAsync(Exception exception, string? additionalText = null, bool shouldSaveInDb = true) =>
            await SaveErrorAsync(exception, additionalText, shouldSaveInDb, (ex) => ex.ToAppErrorDbo(additionalText), new APP_ERROR_CUD(), "unhandled-exceptions-log");

        /// <summary>
        /// Saves user exception,
        /// </summary>
        /// <param name="exception">User exception.</param>
        /// <param name="additionalText">Additional text for exception, can be anything worth mentioning.</param>
        /// <param name="shouldSaveInDb">If should save to database, ot only to file.</param>
        public async Task SaveUserErrorAsync(Exception exception, long userId, string? additionalText = null, bool shouldSaveInDb = true) =>
            await SaveErrorAsync(exception, additionalText, shouldSaveInDb, (ex) => ex.ToUserErrorDbo(userId, additionalText), new USER_ERROR_CUD(), "user-exceptions-log");

        /// <summary>
        /// Main logic for exception saving.
        /// </summary>
        /// <typeparam name="T">Exception type.</typeparam>
        /// <param name="exception">Exception to save.</param>
        /// <param name="additionalText">Additional text to write with exception.</param>
        /// <param name="shouldSaveInDb">If should save to database.</param>
        /// <param name="createModel">Model creation callback.</param>
        /// <param name="cudCommand">CUD repository to use for saving to database.</param>
        /// <param name="fileLogName">File log name.</param>
        /// <exception cref="ArgumentNullException">Is thrown if parameters are null.</exception>
        private async Task SaveErrorAsync<T>(Exception exception, string? additionalText, bool shouldSaveInDb, Func<Exception, T> createModel, ICommandRepository<T> cudCommand, string fileLogName) 
            where T : class
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var model = createModel(exception);
            if (!string.IsNullOrEmpty(additionalText))
            {
                var property = model.GetType().GetProperty("ADDITIONAL_TEXT");
                property?.SetValue(model, additionalText);
            }

            if (shouldSaveInDb)
                await _databaseLogic.CommandAsync(cudCommand, CommandTypes.Create, model);

            var additionalTextFormatted = string.IsNullOrEmpty(additionalText) ? string.Empty : $"\n{additionalText}";
            _simpleLogger.Log(JsonConvert.SerializeObject(exception, Formatting.Indented) + additionalTextFormatted, fileLogName);
        }
    }
}
