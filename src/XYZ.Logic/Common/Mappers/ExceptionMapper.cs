using XYZ.DataAccess.Tables.APP_ERROR_TABLE;
using XYZ.DataAccess.Tables.USER_ERROR_TABLE;

namespace XYZ.Logic.Common.Mappers
{
    public static class ExcceptionMapper
    {
        public static APP_ERROR ToAppErrorDbo(this Exception exception)
        {
            return new APP_ERROR()
            {
                MESSAGE = exception.Message,
                STACKTRACE = exception.StackTrace ?? string.Empty,
            };
        }

        public static USER_ERROR ToUserErrorDbo(this Exception exception, long userId)
        {
            return new USER_ERROR()
            {
                MESSAGE = exception.Message,
                STACKTRACE = exception.StackTrace ?? string.Empty,
                USER_ID = userId,
            };
        }
    }
}
