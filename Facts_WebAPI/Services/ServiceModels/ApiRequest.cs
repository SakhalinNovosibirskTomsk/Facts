using static Facts_Common.SD;

namespace Facts_WebAPI.Service.ServiceModels
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        //public string AccessToken { get; set; }
    }
}
