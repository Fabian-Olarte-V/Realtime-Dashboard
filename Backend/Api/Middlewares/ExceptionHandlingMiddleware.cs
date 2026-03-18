using Api.Contracts.Responses;
using Application.Exceptions;
using FluentValidation;

namespace Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage} ({x.ErrorCode})")
                    .ToList();

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "Validation failed",
                    errors);
            }
            catch (Exception ex)
            {
                var (statusCode, message, errors) = MapException(ex);
                await WriteErrorResponseAsync(context, statusCode, message, errors);
            }
        }

        private static (int StatusCode, string Message, IEnumerable<string>? Errors) MapException(Exception exception)
        {
            return exception switch
            {
                InvalidRequestException ex => (StatusCodes.Status400BadRequest, ex.Message, null),
                InvalidCredentialsException ex => (StatusCodes.Status401Unauthorized, ex.Message, null),
                UnauthorizedActionException ex => (StatusCodes.Status403Forbidden, ex.Message, null),
                EntityNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message, null),
                ConcurrencyConflictException ex => (StatusCodes.Status409Conflict, ex.Message, null),
                InvalidOperationApplicationException ex => (StatusCodes.Status409Conflict, ex.Message, null),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", new[] { "Internal server error" })
            };
        }

        private static Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message, IEnumerable<string>? errors = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiErrorResponse
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors?.ToList() ?? new List<string> { message }
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
