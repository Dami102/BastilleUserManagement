using Newtonsoft.Json;
using System.Net;

namespace BastilleUserService.Core.DTOs.Response
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; } = null!;
        public T Data { get; set; }
        public static APIResponse<T> Fail(string errorMessage, int statusCode = (int)HttpStatusCode.NotFound)
        {
            return new APIResponse<T> { Status = false, Message = errorMessage, StatusCode = statusCode };
        }
        public static APIResponse<T> Success(string successMessage, T data, int statusCode = (int)HttpStatusCode.OK)
        {
            return new APIResponse<T> { Status = true, Message = successMessage, Data = data, StatusCode = statusCode };
        }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
