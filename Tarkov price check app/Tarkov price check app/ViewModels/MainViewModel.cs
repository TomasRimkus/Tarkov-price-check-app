using System.Collections.ObjectModel;
using System.Windows.Input;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Xamarin.Forms;

namespace Tarkov_price_check_app.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
                {
                    var result = await ApiService.ApiServiceInstance.FindItemAsync(text);
                    ObsResults.Clear();

                    foreach (var variable in result.Items)
                    {
                        ObsResults.Add(variable);
                    }
                    SearchResults = result.Result;
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
                    else
                    {
                        _searchresults = "Item not found";
                        OnPropertyChanged();
                    }
                }
                else
                {
                    _searchresults = "Search failed to connect";
                    OnPropertyChanged();
                }
                
            }
        }
        public ObservableCollection<ApiResponseData> ObsCollResults
             {
            get => ObsResults;

            set {
             ObsResults = value;
             OnPropertyChanged();
        }
        }
    }
}
