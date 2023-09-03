namespace Application.Core
{
    // Custom exception that can be thrown when an error is reached, contains the status code of the error, as well as an error message and details (stack trace)
    public class AppException
    {
        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}