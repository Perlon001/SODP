using System.Collections.Generic;

namespace SODP.Domain.Services
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public int StatusCode { get; set; } = 204;
        public IList<string> ValidationErrors { get; set; } = new List<string>();


        public void SetError(string message)
        {
            SetError(message, 500);
        }

        public void SetError(string message, int statusCode)
        {
            Success = false;
            Message += message;
            StatusCode = statusCode;
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }
        public void SetData(T data)
        {
            Data = data;
            StatusCode = 200;
        }
    }

}
