using System;
using System.Collections.Generic;
using System.Text;

namespace Tarkov_price_check_app.Models
{
    public class FullItemsList : IFullItemsList
    {
        public List<ItemsListData> ItemNames { get; set; }
    }
}
