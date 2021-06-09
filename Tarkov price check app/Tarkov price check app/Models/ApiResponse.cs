using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tarkov_price_check_app.Models
{

    public class ApiResponse : IApiResponse
    {
        public List<ApiResponseData> Items { get; set; }
    }

}
