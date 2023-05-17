using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BastilleUserService.Core.DTOs
{
    public class ResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public static ResponseDTO<T> Fail(string message, int statusCode = (int)HttpStatusCode.NotFound)
        {
            return new ResponseDTO<T> { Message= message, Status= false, StatusCode= statusCode };
        }

        public static ResponseDTO<T> Success(string message, T data, int statusCode = (int)HttpStatusCode.OK)
        {
            return new ResponseDTO<T> { Status= true, StatusCode= statusCode, Data = data, Message = message };
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
