using Api.Contracts.Responses;
using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class SuccessResponseResultFilter : IResultFilter
    {
        private readonly IClock _clock;

        public SuccessResponseResultFilter(IClock clock)
        {
            _clock = clock;
        }

        public void OnResultExecuting(ResultExecutingContext context) {
            if (context.Result is not ObjectResult objectResult) return;
            if (objectResult.Value is null) return;

            var statusCode = objectResult.StatusCode ?? 200;
            if (statusCode < 200 || statusCode >= 300) return;


            var valueType = objectResult.Value.GetType();
            var isAlreadyWrapped =
                valueType.IsGenericType &&
                valueType.GetGenericTypeDefinition() == typeof(ApiResponse<>);

            if (isAlreadyWrapped) return;


            var wrappedType = typeof(ApiResponse<>).MakeGenericType(valueType);

            var wrappedValue = Activator.CreateInstance(wrappedType);
            wrappedType.GetProperty(nameof(ApiResponse<object>.Success))!
                    .SetValue(wrappedValue, true);

            wrappedType.GetProperty(nameof(ApiResponse<object>.StatusCode))!
                    .SetValue(wrappedValue, statusCode);

            wrappedType.GetProperty(nameof(ApiResponse<object>.Message))!
                    .SetValue(wrappedValue, "Request processed successfully");

            wrappedType.GetProperty(nameof(ApiResponse<object>.ServerTime))!
                    .SetValue(wrappedValue, _clock.UtcNow);

            wrappedType.GetProperty(nameof(ApiResponse<object>.Data))!
                    .SetValue(wrappedValue, objectResult.Value);


            context.Result = new ObjectResult(wrappedValue)
            {
                StatusCode = statusCode
            };
        }


        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
