using System.Collections.Generic;

namespace SODP.Domain.Services
{
    public class ServicePageResponse<T> : ServiceResponse<PageResponse<T>>
    {
        public ServicePageResponse()
        {
            Data = new PageResponse<T>();
        }

        public void SetData(IList<T> data)
        {
            Data.Collection = data;
            StatusCode = 200;
        }
    }
}