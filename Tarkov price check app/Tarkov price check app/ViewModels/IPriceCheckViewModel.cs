using System.Collections.ObjectModel;
using System.Windows.Input;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.ViewModels
{
    public interface IPriceCheckViewModel
    {
        ICommand ChangeSearchCommand { get; }
        ObservableCollection<ApiResponseData> ObsCollResults { get; set; }
        ICommand SearchCommand { get; }
        string SearchResults { get; set; }
        string SearchText { get; set; }
        ObservableCollection<ItemsListData> SearchTextSuggestions { get; set; }
        bool SuggestionsVisible { get; set; }

        void UpdateSearchText(string text);
    }
}