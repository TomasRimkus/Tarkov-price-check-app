using System.Collections.Generic;

namespace Tarkov_price_check_app.Models
{
    public interface IApiResponse
    {
        List<ApiResponseData> Items { get; set; }
    }
}