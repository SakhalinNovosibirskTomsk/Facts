using Facts_Common;
using Facts_WebAPI.Controllers.Services.IServices;
using Facts_WebAPI.Service.ServiceModels;

namespace Facts_WebAPI.Service
{

    /// <summary>
    /// Реализация интрефейса для запросов данных у сервиса Catalog
    /// </summary>
    public class CatalogService : BaseService, ICatalogService
    {

        private readonly IHttpClientFactory _clientFactory;

        public CatalogService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }


        /// <summary>
        /// Запрос максимального количества дней, на которые можно выдать экземпляр книги
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого отбъекта</typeparam>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает объект ответа сервиса</returns>
        public async Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.CatalogAPIBase + "/BookInstances/GetBookInstanceOutMaxDays/" + id,
                //AccessToken = token
            });
        }

        /// <summary>
        /// Запрос признака, что экземпляр книги выдаётся только в читальный зал
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого отбъекта</typeparam>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает объект ответа сервиса</returns>
        public async Task<T> GetOnlyForReadingRoomByBookInstanceIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.CatalogAPIBase + "/BookInstances/GetBookInstanceOnlyForReadingRoom/" + id,
                //AccessToken = token
            });
        }
    }
}
