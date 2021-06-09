using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.Services
{
    public class TarkovMarketApiService : ITarkovMarketApiService
    {
        private readonly TarkovMarketApiService _apiServiceInstance = new TarkovMarketApiService();
        private static readonly HttpClient Client = new HttpClient();

        public TarkovMarketApiService ApiServiceInstance
        {
            get
            {
                return _apiServiceInstance;
            }
        }

        public async Task<ApiResponse> FindItem(string query)
        {
            ApiResponse ResponseList = new ApiResponse();
            string ApiKey = GrabApiKey();
            string encodedQuery = HttpUtility.UrlEncode(query);
            var result = await Client.GetStringAsync($"https://tarkov-market.com/api/v1/item?q={encodedQuery}&x-api-key={ApiKey}");
            ResponseList.Items = JsonConvert.DeserializeObject<List<ApiResponseData>>(result);

            return ResponseList;
        }

        public async Task<FullItemsList> GetAllItemNames()
        {
            string ApiKey = GrabApiKey();
            FullItemsList ItemNames = new FullItemsList();
            Client.DefaultRequestHeaders.Add("x-api-key", ApiKey);
            var result = await Client.GetStringAsync($"https://tarkov-market.com/api/v1/items/all");
            ItemNames.ItemNames = JsonConvert.DeserializeObject<List<ItemsListData>>(result);
            return ItemNames;
        }


        private string GrabApiKey()
        {
            var fileName = "Tarkov_price_check_app.Resources.SensitiveData.txt";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fileName);

            if (stream == null)
            {
                throw new FileNotFoundException("Cannot find API key file.", fileName);
            }
            string key = "";


            using (var reader = new System.IO.StreamReader(stream))
            {
                key = reader.ReadToEnd();
            }

            return key;
        }

    }
}
