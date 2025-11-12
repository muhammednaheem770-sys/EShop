
namespace EShop.Dto
{
    public class BaseResponse<T>
    {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T? Data { get; set; }

            public BaseResponse(bool success, string message, T data = default)
            {
                Success = success;
                Message = message;
                Data = data;
            }

            public BaseResponse(T? data, string message, bool success = true)
            {
                Data = data; Success = success;
                Message = message;
                Success = success;
            }

            public BaseResponse(bool success, string message)
            {
                Success = success;
                Message = message;
            }



        public static BaseResponse<T> SuccessResponse(T data, string message = "Success")
                => new(data, message, true);

            public static BaseResponse<T> FailureResponse(string message)
                => new(default, message, false);
    }
}
