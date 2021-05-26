using System.Collections.ObjectModel;
using System.Windows.Input;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Xamarin.Forms;

namespace Tarkov_price_check_app.ViewModels
{
    public class ItemSearchViewModel : BindableObject
    {
        private ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
                {
                    var result = await TarkovMarketApiService.ApiServiceInstance.FindItemAsync(text);
                    ObsResults.Clear();

                    foreach (var variable in result.Items)
                    {
                        string tempName = variable.Name.ToUpper();
                        string tempSearch = text.ToUpper();
                        if (tempName.Contains(tempSearch))
                        ObsResults.Add(variable);
                    }

                    if (result.Items.Count > 0)
                        SearchResults = "ok";
                    else
                        SearchResults = "Empty";
                }));
            }
        }

        public ObservableCollection<ApiResponseData> ObsResults = new ObservableCollection<ApiResponseData>();

        private string _searchresults = "";


        public string SearchResults
        {
            get => _searchresults;
            set
            {
                if (value == "ok")
                {
                    if (ObsResults.Count == 1)
                    {
                        _searchresults = "Found 1 item.";
                        OnPropertyChanged();
                    }

                    else if (ObsResults.Count > 1)
                    {
                        _searchresults = $"Found {ObsResults.Count} items.";
                        OnPropertyChanged();
                    }
                }
                else if (value == "Empty")
                {
                    _searchresults = "Item not found";
                    OnPropertyChanged();
                }

            }
        }
        public ObservableCollection<ApiResponseData> ObsCollResults
        {
            get => ObsResults;

            set
            {
                ObsResults = value;
                OnPropertyChanged();
            }
        }
    }
}
