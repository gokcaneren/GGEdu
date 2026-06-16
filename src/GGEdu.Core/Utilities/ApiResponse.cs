using GGEdu.Core.DTOs.Courses.Input;
using GGEdu.Core.DTOs.Courses.Output;
using System.Net;

namespace GGEdu.Core.Utilities
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(HttpStatusCode httpStatusCode, string message, T? data)
            => new ApiResponse<T> { Success = true, StatusCode = httpStatusCode, Message = message, Data = data };
        
        public static ApiResponse<T> ErrorResponse(HttpStatusCode httpStatusCode, string message, T? data)
            => new ApiResponse<T> { Success = false, StatusCode = httpStatusCode, Message = message, Data = data };
    }
}
