using Newtonsoft.Json;

namespace Tarkov_price_check_app.Models
{
    public class ItemsListData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
