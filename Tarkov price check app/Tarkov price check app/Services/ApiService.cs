using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.Services
{
    public class ApiService
    {
        private static ApiService _apiServiceInstance;

        public static ApiService ApiServiceInstance
        {
            get
            {
                if (_apiServiceInstance == null)
                    _apiServiceInstance = new ApiService();
                return _apiServiceInstance;
            }
        }

        private HttpClient _client;


        public ApiService()
        {
            _client = new HttpClient();
        }
        public async Task<ApiResponse> FindItemAsync(string query)
        {

            string encodedQuery = HttpUtility.UrlEncode(query);
                var result = await _client.GetStringAsync($"https://tarkov-market.com/api/items?lang=en&search={encodedQuery}&limit=20");

        ApiResponse objresult = JsonConvert.DeserializeObject<ApiResponse>(result);


        if (!string.IsNullOrEmpty(objresult.Result))
        {
            objresult.Items.RemoveAll(x => !x.EnName.ToUpper().Contains(query.ToUpper()));
        }

        return objresult;
        }

}

}
