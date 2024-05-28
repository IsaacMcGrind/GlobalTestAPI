using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Security;
using System.Text.Json;

namespace GlobalTestAPI.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception != null)
            {
                _logger.LogError(exception, "An unhandled exception has occurred.");

                httpContext.Response.ContentType = "application/json";
                HttpStatusCode statusCode;
                string? message;

                switch (exception)
                {
                    case ArgumentException:
                        statusCode = HttpStatusCode.BadRequest;
                        message = exception.Message;
                        break;
                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        message = exception.Message;
                        break;
                    case NullReferenceException:
                        statusCode = HttpStatusCode.InternalServerError;
                        message = "A null reference error occurred.";
                        break;
                    case InvalidOperationException:
                        statusCode = HttpStatusCode.Conflict;
                        message = exception.Message;
                        break;
                    case NotImplementedException:
                        statusCode = HttpStatusCode.NotImplemented;
                        message = "This functionality is not yet implemented.";
                        break;
                    case TimeoutException:
                        statusCode = HttpStatusCode.RequestTimeout;
                        message = "The operation timed out.";
                        break;
                    case FormatException:
                        statusCode = HttpStatusCode.BadRequest;
                        message = "A format error occurred.";
                        break;
                    case IndexOutOfRangeException:
                        statusCode = HttpStatusCode.BadRequest;
                        message = "An index was out of range.";
                        break;
                    case KeyNotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        message = "The specified key was not found.";
                        break;
                    case IOException:
                        statusCode = HttpStatusCode.ServiceUnavailable;
                        message = "An I/O error occurred.";
                        break;
                    case SecurityException:
                        statusCode = HttpStatusCode.Forbidden;
                        message = "A security error occurred.";
                        break;
                    case StackOverflowException:
                        statusCode = HttpStatusCode.InternalServerError;
                        message = "A stack overflow error occurred.";
                        break;
                    case OutOfMemoryException:
                        statusCode = HttpStatusCode.InternalServerError;
                        message = "The application ran out of memory.";
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        message = "An internal server error has occurred.";
                        break;
                }


                httpContext.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    httpContext.Response.StatusCode,
                    Message = message,
                    Detailed = exception.Message
                };

                var responseJson = JsonSerializer.Serialize(response);
                await httpContext.Response.WriteAsync(responseJson, cancellationToken);

                return true;
            }

            return false;
        }
    }
}
