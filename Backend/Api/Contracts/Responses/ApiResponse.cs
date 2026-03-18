namespace Api.Contracts.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public int StatusCode { get; set; }
        public string Message { get; init; } = string.Empty;
        public DateTimeOffset ServerTime { get; set; }
        public T? Data { get; init; }
    }
}
