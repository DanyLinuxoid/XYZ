using Newtonsoft.Json;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
using XYZ.DataAccess.Tables.Base;
using XYZ.DataAccess.Tables.USER_ERROR_TABLE;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Common.Mappers;

namespace XYZ.Logic.Common.ExceptionSaving
{
    public class ExceptionSaverLogic : IExceptionSaverLogic
    {
        private readonly IDatabaseLogic _databaseLogic;
        private readonly ISimpleLogger _simpleLogger;

        public ExceptionSaverLogic(IDatabaseLogic databaseLogic, ISimpleLogger simpleLogger)
        {
            _databaseLogic = databaseLogic;
            _simpleLogger = simpleLogger;
        }

        public async Task SaveUnhandledErrorAsync(Exception exception, string? additionalText = null, bool shouldSaveInDb = true) => 
            await SaveError(exception, "unhandled-exceptions-log", new APP_ERROR_CUD(), exception.ToAppErrorDbo(), shouldSaveInDb, additionalText);

        public async Task SaveUserErrorAsync(Exception exception, long userId, string? additionalText = null, bool shouldSaveInDb = true) => 
            await SaveError(exception, "user-exceptions-log", new USER_ERROR_CUD(), exception.ToUserErrorDbo(userId), shouldSaveInDb, additionalText);

        private async Task SaveError<T>(Exception exception, string logName, ICommandRepository<T> repository, T model, bool shouldSaveInDb, string? additionalText = null) 
            where T : ERROR_BASE
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (!string.IsNullOrEmpty(additionalText))
                model.ADDITIONAL_TEXT = additionalText;

            if (shouldSaveInDb)
                await _databaseLogic.CommandAsync(repository, CommandTypes.Create, model);

            var additionalTextFormatted = string.IsNullOrEmpty(additionalText) ? string.Empty : $"\n{additionalText}";
            _simpleLogger.Log(JsonConvert.SerializeObject(exception, Formatting.Indented) + additionalTextFormatted, logName);
        }
    }
}
