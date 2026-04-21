namespace OskApi.Shared.Result
{
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static Result Ok(string? message = null)
            => new Result { Success = true, Message = message };

        public static Result Fail(string message)
            => new Result { Success = false, Message = message };
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public static Result<T> Ok(T data, string? message = null)
            => new Result<T> { Success = true, Data = data, Message = message };

        public static new Result<T> Fail(string message)
            => new Result<T> { Success = false, Message = message };
    }
}
