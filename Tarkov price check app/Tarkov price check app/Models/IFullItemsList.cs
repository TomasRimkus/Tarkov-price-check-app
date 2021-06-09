using System.Collections.Generic;

namespace Tarkov_price_check_app.Models
{
    public interface IFullItemsList
    {
        List<ItemsListData> ItemNames { get; set; }
    }
}