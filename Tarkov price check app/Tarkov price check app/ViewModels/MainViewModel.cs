using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
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
                    ObsResults1.Clear();

                    foreach (var Variable in result.items)
                    {
                        ObsResults1.Add(Variable);
                    }
                    ObsCollResults = ObsResults1;
                    SearchResults = result.result;
                }));
            }
        }

        public ObservableCollection<ApiResponseData> ObsResults = new ObservableCollection<ApiResponseData>();
        public ObservableCollection<ApiResponseData> ObsResults1 = new ObservableCollection<ApiResponseData>();

        private string Searchresults = "";


        public string SearchResults
        {
            get { return Searchresults; }
            set
            {
                if (value == "ok")
                {
                    if (ObsResults.Count == 1)
                    {
                        Searchresults = "Found 1 item.";
                        OnPropertyChanged();
                    }

                    else if (ObsResults.Count > 1)
                    {
                        Searchresults = $"Found {ObsResults.Count.ToString()} items.";
                        OnPropertyChanged();
                    }
                    else
                    {
                        Searchresults = "Item not found";
                        OnPropertyChanged();
                    }
                }
                else
                {
                    Searchresults = "Search failed to connect";
                    OnPropertyChanged();
                }
                
            }
        }
        public ObservableCollection<ApiResponseData> ObsCollResults
             {
            get { return ObsResults; }

            set {
             ObsResults = value;
             OnPropertyChanged();
        }
        }
    }
}
