using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var cliente = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                switch(apiRequest.ApiType)
                {
                    case Utility.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case Utility.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case Utility.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apipResponse = null;
                apipResponse = await cliente.SendAsync(message);
                var apiContent = await apipResponse.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponse;
            }
            catch(Exception ex)
            {
                var ErrorMessage = new APIResponse
                {
                    Errors = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(ErrorMessage);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);

                return apiResponse;
            }
        }
    }
}
