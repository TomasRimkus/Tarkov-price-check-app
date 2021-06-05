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

        private readonly HttpClient _client;

        public TarkovMarketApiService()
        {
            _client = new HttpClient();
        }

        public async Task<ApiResponse> FindItemAsync(string query)
        {
            SensitiveData SensData = new SensitiveData();
            ApiResponse ResponseList = new ApiResponse();
            string encodedQuery = HttpUtility.UrlEncode(query);

            var result = await _client.GetStringAsync($"https://tarkov-market.com/api/v1/item?q={encodedQuery}&x-api-key={SensData.ApiKey}");
            ResponseList.Items = JsonConvert.DeserializeObject<List<ApiResponseData>>(result);

            return ResponseList;
        }
    }

    public class SensitiveData
    {
        public string ApiKey { get; set; }
    }
}
