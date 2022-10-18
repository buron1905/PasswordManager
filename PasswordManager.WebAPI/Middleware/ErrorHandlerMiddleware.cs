using Services.Abstraction;
using Services.Abstraction.Exceptions;
using System.Net;
using System.Text.Json;

namespace PasswordManager.WebAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _loggerManager;

        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerManager loggerManager)
        {
            _next = next;
            _loggerManager = loggerManager;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _loggerManager.LogError($"Something went wrong: {error}");
                var response = context.Response;
                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";

                    switch (error)
                    {
                        case AppException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case KeyNotFoundException e:
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        default:
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
