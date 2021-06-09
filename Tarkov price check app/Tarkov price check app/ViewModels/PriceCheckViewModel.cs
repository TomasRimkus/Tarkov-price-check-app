using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Xamarin.Forms;

namespace Tarkov_price_check_app.ViewModels
{
    public class PriceCheckViewModel : BindableObject, IPriceCheckViewModel
    {
        ITarkovMarketApiService _tarkovMarketApiService;
        public PriceCheckViewModel(ITarkovMarketApiService tarkovMarketApiService)
        {
            _tarkovMarketApiService = tarkovMarketApiService;
            Task.Run(() => this.HandleNamesList()).Wait();
        }

        private string searchText;

        private ICommand _searchCommand;
        private ICommand _changeSearchCommand;

        private string _searchresults = "";

        private bool suggestionsVisible = true;

        public ObservableCollection<ApiResponseData> ObsResults = new ObservableCollection<ApiResponseData>();
        public ObservableCollection<ItemsListData> ObsItemNames;
        public ObservableCollection<ItemsListData> searchTextSuggestions = new ObservableCollection<ItemsListData>();
        private ObservableCollection<ItemsListData> tempSearchTextSuggestions = new ObservableCollection<ItemsListData>();

        public ICommand ChangeSearchCommand
        {
            get
            {
                return _changeSearchCommand ?? (_changeSearchCommand = new Command<string>((text) =>
                {
                    UpdateSearchText(text);
                    SearchCommand.Execute(text);
                }));
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
                {
                    var result = await _tarkovMarketApiService.FindItem(text);
                    ObsResults.Clear();
                    SuggestionsVisible = false;

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

        private async Task HandleNamesList()
        {
            var result = await _tarkovMarketApiService.GetAllItemNames();
            ObsItemNames = new ObservableCollection<ItemsListData>(result.ItemNames.Distinct().ToList());
        }

        public void UpdateSearchText(string text)
        {
            SearchText = text;
            tempSearchTextSuggestions.Clear();
        }

        private void PopulateSuggestionList()
        {
            if (SearchText.Length > 2)
            {
                tempSearchTextSuggestions.Clear();
                foreach (var item in ObsItemNames)
                {
                    if (item.Name.ToUpper().Contains(SearchText.ToUpper()))
                    {
                        tempSearchTextSuggestions.Add(item);
                    }
                }
                SearchTextSuggestions = tempSearchTextSuggestions;
            }
        }

        public ObservableCollection<ItemsListData> SearchTextSuggestions
        {
            get => searchTextSuggestions;
            set
            {
                searchTextSuggestions = value;
                OnPropertyChanged();
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

        public bool SuggestionsVisible
        {
            get => suggestionsVisible;

            set
            {
                suggestionsVisible = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                PopulateSuggestionList();
                SuggestionsVisible = true;
                OnPropertyChanged();
            }
        }

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

    }
}
