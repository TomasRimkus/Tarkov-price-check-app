using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tarkov_price_check_app.Models
{
    public class ApiResponseData
    {

            public string enImg { get; set; }
           
            public string enName { get; set; }

            public int price { get; set; }

            public string updated { get; set; }

            public int avgDayPrice { get; set; }

            public string traderName { get; set; }

            public int traderPrice { get; set; }
    }

    public class ApiResponse
    {
        public string result { get; set; }
        public List<ApiResponseData> items { get; set; }
    }
}
