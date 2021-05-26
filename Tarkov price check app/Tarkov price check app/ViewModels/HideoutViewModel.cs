using System;
using System.Collections.Generic;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Tarkov_price_check_app.Views;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Tarkov_price_check_app.ViewModels
{
    public class HideoutViewModel : BindableObject
    {
        public HideoutItems.FullItems Results = new HideoutItems.FullItems();

        public ObservableCollection<HideoutItems.Item> IntelData = new ObservableCollection<HideoutItems.Item>();
        public ObservableCollection<HideoutItems.Item> LavaData = new ObservableCollection<HideoutItems.Item>();
        public ObservableCollection<HideoutItems.Item> WorkData = new ObservableCollection<HideoutItems.Item>();

        public AsyncCommand RefreshCommand { get; }

        public HideoutViewModel()
        {
            RefreshCommand = new AsyncCommand(Refresh);
        }

        private async Task Refresh()
        {
            await CalcPrices();
        }

        public void LoadJson()
        {

            var assembly = typeof(HideoutPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.Contains("data.json"))
                {
                    var stream = assembly.GetManifestResourceStream(res);

                    using (var r = new StreamReader(stream))
                    {
                        var json = r.ReadToEnd();
                        Results = JsonConvert.DeserializeObject<HideoutItems.FullItems>(json);
                    }
                }
            }
        }

        public async Task CalcPricesAsync()
        {
            foreach (var item in Results.Items)
            {
                item.ResultProfit = 0;
                int ingredientTotalPrice = 0;

                for (int i = 0; i < item.Ingredients.Ingredient.Count; i++)
                {
                    var priceList = await TarkovMarketApiService.ApiServiceInstance.FindItemAsync(item.Ingredients.Ingredient[i]);
                    ingredientTotalPrice += item.Ingredients.IngredientAmount[i] * priceList.Items[0].Avg24hPrice;
                }

                var resultPriceList = await TarkovMarketApiService.ApiServiceInstance.FindItemAsync(item.ResultItemName);
                int resultProfit = item.ResultCount * resultPriceList.Items[0].Avg24hPrice;
                item.ResultProfit = resultProfit - ingredientTotalPrice;
            }
        }

        public void MoveItemsToStations()
        {
            foreach (var item in Results.Items)
            {
                switch (item.Station)
                {
                    case HideoutItems.Stations.Intel:
                        IntelData.Add(item);
                        break;
                    case HideoutItems.Stations.Lav:
                        LavaData.Add(item);
                        break;
                    case HideoutItems.Stations.Work:
                        WorkData.Add(item);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task CalcPrices()
        {

            IntelData.Clear();
            LavaData.Clear();
            WorkData.Clear();

            SetStatus = "Updating prices...";
            SetButtonStatus = false;
            LoadJson();
           //neat stuff> await Task.WhenAll(new List<Task> { CalcPricesAsync(), CalcPricesAsync2() });
            await CalcPricesAsync();
            MoveItemsToStations();
            SetStatus = "Done";
            SetButtonStatus = true;
        }

        public ObservableCollection<HideoutItems.Item> IntelDataCollection
        {
            get => IntelData;

            set
            {
                IntelData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HideoutItems.Item> LavDataCollection
        {
            get => LavaData;

            set
            {
                LavaData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HideoutItems.Item> WorkDataCollection
        {
            get => WorkData;

            set
            {
                WorkData = value;
                OnPropertyChanged();
            }
        }


        public string Status = "Ready";
        public bool ButtonStatus = true;


        public string[] StationsList = new string[3] { "Lavatory", "Intel Center", "Workbench" };

        public bool IntelVisible = false;
        public bool LavVisible = false;
        public bool WorkVisible = false;
        public int CurrentStation = -1;

        public string SetStatus
        {
            get => Status;
            set
            {
                Status = value;
                OnPropertyChanged();
            }
        }

        public bool SetButtonStatus
        {
            get => ButtonStatus;
            set
            {
                ButtonStatus = value;
                OnPropertyChanged();
            }
        }

        public bool IsIntelVisible
        {
            get => IntelVisible;
            set
            {
                IntelVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsLavVisible
        {
            get => LavVisible;
            set
            {
                LavVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsWorkVisible
        {
            get => WorkVisible;
            set
            {
                WorkVisible = value;
                OnPropertyChanged();
            }
        }

        public int SetCurrentStation
        {
            get => CurrentStation;
            set
            {
                CurrentStation = value;
                OnPropertyChanged();

                if (value == 0)
                {
                    IsLavVisible = true;
                    IsIntelVisible = false;
                    IsWorkVisible = false;
                }
                else if (value == 1)
                {
                    IsIntelVisible = true;
                    IsLavVisible = false;
                    IsWorkVisible = false;
                }
                else if (value == 2)
                {
                    IsWorkVisible = true;
                    IsLavVisible = false;
                    IsIntelVisible = false;
                }
            }
        }


        public string[] GetStationsList
        {
            get => StationsList;
        }
    }
}
