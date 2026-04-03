using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Shared.ApiModels
{
    public class ApiResponse
    {
        public bool IsSuccessful { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public ApiResponse(bool isSuccessful, string? errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public static ApiResponse Success()
        {
            return new ApiResponse(true, null);
        }

        public static ApiResponse Fail(string? errorMessage)
        {
            return new ApiResponse(false, errorMessage);
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Response { get; set; }

        public ApiResponse(bool isSuccessful, string? errorMessage, T? response)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Response = response;
        }

        public static ApiResponse<T> Success(T response)
        {
            return new ApiResponse<T>(true, null, response);
        }

        public static ApiResponse<T> Fail(string? errorMessage)
        {
            return new ApiResponse<T>(false, errorMessage, default(T));
        }
    }
}
