using Newtonsoft.Json;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
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
            await SaveErrorAsync(exception, additionalText, shouldSaveInDb, (ex) => ex.ToAppErrorDbo(), new APP_ERROR_CUD(), "unhandled-exceptions-log");

        public async Task SaveUserErrorAsync(Exception exception, long userId, string? additionalText = null, bool shouldSaveInDb = true) =>
            await SaveErrorAsync(exception, additionalText, shouldSaveInDb, (ex) => ex.ToUserErrorDbo(userId), new USER_ERROR_CUD(), "user-exceptions-log");

        private async Task SaveErrorAsync<T>(Exception exception, string? additionalText, bool shouldSaveInDb, Func<Exception, T> createModel, ICommandRepository<T> cudCommand, string logType) 
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
            _simpleLogger.Log(JsonConvert.SerializeObject(exception, Formatting.Indented) + additionalTextFormatted, logType);
        }
    }
}
