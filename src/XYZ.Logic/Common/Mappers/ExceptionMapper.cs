using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
using XYZ.DataAccess.Tables.USER_ERROR_TABLE;

namespace XYZ.Logic.Common.Mappers
{
    /// <summary>
    /// Application exception mapping to dbo for saving in database.
    /// </summary>
    public static class ExcceptionMapper
    {
        /// <summary>
        /// Maps exception to application level exception.
        /// </summary>
        /// <param name="exception">Any exception.</param>
        /// <returns>Application level exception.</returns>
        public static APP_ERROR ToAppErrorDbo(this Exception exception, string? additionalText = null)
        {
            return new APP_ERROR()
            {
                MESSAGE = exception.Message,
                STACKTRACE = exception.StackTrace ?? string.Empty,
                ADDITIONAL_TEXT = additionalText,
            };
        }

        /// <summary>
        /// Maps exception to user level exception.
        /// </summary>
        /// <param name="exception">Any exception.</param>
        /// <returns>User level exception.</returns>
        public static USER_ERROR ToUserErrorDbo(this Exception exception, long userId, string? additionalText = null)
        {
            return new USER_ERROR()
            {
                MESSAGE = exception.Message,
                STACKTRACE = exception.StackTrace ?? string.Empty,
                USER_ID = userId,
                ADDITIONAL_TEXT = additionalText,
            };
        }
    }
}
