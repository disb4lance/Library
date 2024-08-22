using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Requests
{
    public class ApiResponse
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        private ApiResponse(bool success, string message, object data = null)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse Success(object data = null)
        {
            return new ApiResponse(true, string.Empty, data);
        }

        public static ApiResponse Failure(string message)
        {
            return new ApiResponse(false, message);
        }
    }
}
