namespace Application.Core
{
    // Result object that facilitates error handling. Takes in the value for a success and the error message for a failure
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value, };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
    }
}