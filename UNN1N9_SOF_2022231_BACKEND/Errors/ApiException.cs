namespace UNN1N9_SOF_2022231_BACKEND.Errors
{
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public ApiException(int statusCode, string message, string description)
        {
            StatusCode = statusCode;
            Message = message;
            Description = description;
        }
    }
}
