using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tarkov_price_check_app.Models
{
    public class ApiResponseData
    {

            public string EnImg { get; set; }
           
            public string EnName { get; set; }

            public int Price { get; set; }

            public string Updated { get; set; }

            public int AvgDayPrice { get; set; }

            public string TraderName { get; set; }

            public int TraderPrice { get; set; }
    }

    public class ApiResponse
    {
        public string Result { get; set; }
        public List<ApiResponseData> Items { get; set; }
    }
}
