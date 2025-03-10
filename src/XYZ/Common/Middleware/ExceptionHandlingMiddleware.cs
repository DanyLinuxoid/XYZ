namespace XYZ.Web.Common.Middleware
{
    using Microsoft.AspNetCore.Http;

    using System;
    using System.Threading.Tasks;

    using XYZ.Logic.Common.Interfaces;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private IExceptionSaverLogic _exceptionSaverLogic;

        public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionSaverLogic exceptionSaverLogic)
        {
            _next = next;
            _exceptionSaverLogic = exceptionSaverLogic;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string unhandledErrorCode = Guid.NewGuid().ToString();

                // Prepare response
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                string jsonResponse = $"{{\"message\": \"Something went wrong\", \"code\": \"{unhandledErrorCode}\"}}";
                await context.Response.WriteAsync(jsonResponse);
                await SafeLogExceptionWithFallback(ex, unhandledErrorCode, context);
            }
        }

        private async Task SafeLogExceptionWithFallback(Exception ex, string errorCode, HttpContext context)
        {
            string additionalText =
                $"{errorCode}\n" +
                $"({context.Request.Method}) {context.Request.Path}";

            try // Db + file save
            {
                await _exceptionSaverLogic.SaveUnhandledErrorAsync(ex, additionalText: additionalText);
            }
            catch (Exception dbEx)
            {
                try // System file fallback
                {
                    await _exceptionSaverLogic.SaveUnhandledErrorAsync(
                        new AggregateException($"Database unhandled exception logging failed", dbEx, ex),
                        additionalText: additionalText,
                        shouldSaveInDb: false);
                }
                catch (Exception fileEx)
                {
                    try // Event viewer fallback
                    {
                        System.Diagnostics.Trace.WriteLine(
                            $"Failed logging to database + file\n\n" +
                            $"{dbEx}\n\n" +
                            $"{fileEx.ToString()}\n\n" +
                            $"{additionalText}");
                    }
                    catch { }
                }
            }
        }
    }
}
