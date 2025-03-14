using Facts_Common;
using Facts_WebAPI.Controllers.Services.IServices;
using Facts_WebAPI.Service.ServiceModels;
using Newtonsoft.Json;
using System.Text;

namespace Facts_WebAPI.Service
{

    /// <summary>
    /// Реализация базового класса запроса к сторонним сервисам
    /// </summary>
    public class BaseService : IBaseService
    {
        /// <summary>
        /// Модель ответа
        /// </summary>
        public ResponseDTO responseModel { get; set; }

        /// <summary>
        /// http-клиент
        /// </summary>
        public IHttpClientFactory httpClient { get; set; }


        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDTO();
            this.httpClient = httpClient;
        }


        /// <summary>
        /// Запрос по REST API
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="apiRequest">Объект с параметрами запроса</param>
        /// <returns>Возвращает объект, полученный в результате запроса</returns>
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("FactsAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                //message.Headers.Add("Accept", "text/plain");
                message.RequestUri = new Uri(apiRequest.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    //message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "text/plain");
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                //-- Вроде этого не должно быть, но без этого не работает -->
                apiResponseDto.GetType().GetProperty("Result").SetValue(apiResponseDto, apiContent);
                //-- Вроде этого не должно быть, но без этого не работате <--
                return apiResponseDto;
            }
            catch (Exception e)
            {
                var dto = new ResponseDTO
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDTO;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
