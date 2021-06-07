using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Resources;

namespace Tarkov_price_check_app.Services
{
    public class TarkovMarketApiService
    {
        private static TarkovMarketApiService _apiServiceInstance = new TarkovMarketApiService();
        private static HttpClient Client = new HttpClient();

        public static TarkovMarketApiService ApiServiceInstance
        {
            get
            {
                return _apiServiceInstance;
            }
        }

        public async Task<ApiResponse> FindItem(string query)
        {
            SensitiveData SensData = new SensitiveData();
            ApiResponse ResponseList = new ApiResponse();
            string encodedQuery = HttpUtility.UrlEncode(query);

            var result = await Client.GetStringAsync($"https://tarkov-market.com/api/v1/item?q={encodedQuery}&x-api-key={SensData.ApiKey}");
            ResponseList.Items = JsonConvert.DeserializeObject<List<ApiResponseData>>(result);

            return ResponseList;
        }

        public async Task<FullItemsList> GetAllItemNames()
        {
            SensitiveData SensData = new SensitiveData();
            FullItemsList ItemNames = new FullItemsList();
            Client.DefaultRequestHeaders.Add("x-api-key", SensData.ApiKey);

            var result = await Client.GetStringAsync($"https://tarkov-market.com/api/v1/items/all");
            ItemNames.ItemNames = JsonConvert.DeserializeObject<List<ItemsListData>>(result);
            return ItemNames;
        }


    }
}
