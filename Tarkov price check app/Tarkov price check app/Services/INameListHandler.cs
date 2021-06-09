using System.Collections.Generic;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.Services
{
    public interface INameListHandler
    {
        List<ItemsListData> SavedList { get; set; }
    }
}