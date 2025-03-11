using Facts_Common;
using Facts_WebAPI.Controllers.Services.IServices;
using Facts_WebAPI.Service.ServiceModels;

namespace Facts_WebAPI.Service
{
    public class CatalogService : BaseService, ICatalogService
    {

        private readonly IHttpClientFactory _clientFactory;

        public CatalogService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }


        //public async Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id, string token)
        public async Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.CatalogAPIBase + "/BookInstances/GetBookInstanceOutMaxDays/" + id,
                //AccessToken = token
            });
        }

        //public async Task<T> GetOnlyForReadingRoomByBookInstanceIdAsync<T>(int id, string token)
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
