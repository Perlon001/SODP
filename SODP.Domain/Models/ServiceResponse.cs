using System.Collections.Generic;

namespace SODP.Domain.Models
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public int StatusCode { get; set; } = 204;
        public IDictionary<string,string> ValidationErrors { get; set; } = new Dictionary<string,string>();


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
