namespace Api.Contracts.Responses
{
    public class ApiErrorResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; set; }
        public string Message { get; init; } = string.Empty;
        public List<string> Errors { get; init; } = new();
    }
}
