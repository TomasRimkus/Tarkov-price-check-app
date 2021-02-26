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
        private static ApiService _ApiServiceInstance;

        public static ApiService ApiServiceInstance
        {
            get
            {
                if (_ApiServiceInstance == null)
                    _ApiServiceInstance = new ApiService();
                return _ApiServiceInstance;
            }
        }

        private HttpClient client;


        public ApiService()
        {
            client = new HttpClient();
        }
        public async Task<ApiResponse> FindItemAsync(string query)
        {

            string encodedQuery = HttpUtility.UrlEncode(query);
                var result = await client.GetStringAsync($"https://tarkov-market.com/api/items?lang=en&search={encodedQuery}&limit=20");

        ApiResponse objresult = JsonConvert.DeserializeObject<ApiResponse>(result);


        if (!string.IsNullOrEmpty(objresult.result))
        {
            objresult.items.RemoveAll(x => !x.enName.ToUpper().Contains(query.ToUpper()));
        }

        return objresult;
        }

}

}
