using System.Threading.Tasks;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.Services
{
    public interface ITarkovMarketApiService
    {
        Task<ApiResponse> FindItem(string query);
        Task<FullItemsList> GetAllItemNames();
    }
}