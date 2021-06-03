using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.Services
{
    public class TarkovMarketApiService
    {
        private static TarkovMarketApiService _apiServiceInstance;

        public static TarkovMarketApiService ApiServiceInstance
        {
            get
            {
                if (_apiServiceInstance == null)
                    _apiServiceInstance = new TarkovMarketApiService();
                return _apiServiceInstance;
            }
        }

        private HttpClient _client;
        private ApiResponse ResponseList = new ApiResponse();


        public TarkovMarketApiService()
        {
            _client = new HttpClient();
        }

        public async Task<ApiResponse> FindItemAsync(string query)
        {

            string encodedQuery = HttpUtility.UrlEncode(query);
            var result = await _client.GetStringAsync($"https://tarkov-market.com/api/v1/item?q={encodedQuery}&x-api-key=");

            var objresult = JsonConvert.DeserializeObject<List<ApiResponseData>>(result);
            ResponseList.Items = objresult;

            return ResponseList;
        }

    }

}
