namespace SODP.Domain.Services
{
    public class ServicePageResponse<T> : ServiceResponse<PageResponse<T>>
    {
        public ServicePageResponse()
        {
            Data = new PageResponse<T>();
        }
    }
}