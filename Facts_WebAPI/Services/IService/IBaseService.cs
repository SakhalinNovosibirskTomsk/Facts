using Facts_WebAPI.Service.ServiceModels;

namespace Facts_WebAPI.Controllers.Services.IServices
{
    /// <summary>
    /// Базовый сервис запросов к другим сервисам
    /// </summary>
    public interface IBaseService : IDisposable
    {
        ResponseDTO responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
