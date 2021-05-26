using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tarkov_price_check_app.Models
{
    public class ApiResponseData
    {

        [JsonProperty("img")]
        public string Img { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("avg24hPrice")]
        public int Avg24hPrice { get; set; }

        [JsonProperty("traderName")]
        public string TraderName { get; set; }

        [JsonProperty("traderPrice")]
        public int TraderPrice { get; set; }
    }
}
