using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Requests
{
    public class ApiResponse
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        private ApiResponse(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static ApiResponse Success(string message = "Operation completed successfully.")
        {
            return new ApiResponse(true, message);
        }

        public static ApiResponse Failure(string errorMessage)
        {
            return new ApiResponse(false, errorMessage);
        }
    }
}
