using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tarkov_price_check_app.Services;
using Xamarin.Forms;
using static Tarkov_price_check_app.Models.HideoutItems;

namespace Tarkov_price_check_app.ViewModels
{
    public class HideoutViewModel : BindableObject, IHideoutViewModel
    {
        readonly ITarkovMarketApiService _tarkovMarketApiService;
        public HideoutViewModel(ITarkovMarketApiService tarkovMarketApiService)
        {
            _tarkovMarketApiService = tarkovMarketApiService;
            RefreshCommand = new AsyncCommand(Refresh);
        }

        private FullItems Results = new FullItems();

        private ObservableCollection<Item> IntelData = new ObservableCollection<Item>();
        private ObservableCollection<Item> LavaData = new ObservableCollection<Item>();
        private ObservableCollection<Item> WorkData = new ObservableCollection<Item>();


        private string Status = "Ready";
        private bool ButtonStatus = true;


        private readonly string[] StationsList = new string[3] { "Lavatory", "Intel Center", "Workbench" };

        private bool IntelVisible = false;
        private bool LavVisible = false;
        private bool WorkVisible = false;
        private int CurrentStation = -1;

        public AsyncCommand RefreshCommand { get; }

        private async Task Refresh()
        {
            await CalcPrices();
        }

        private void LoadJson()
        {
            var fileName = "Tarkov_price_check_app.Resources.data.json";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fileName);

            if (stream == null)
            {
                throw new FileNotFoundException("Cannot find Hideout data file.", fileName);
            }
            using (var r = new StreamReader(stream))
            {
                var json = r.ReadToEnd();
                Results = JsonConvert.DeserializeObject<FullItems>(json);
            }
            foreach (var item in Results.Items)
            {
                item.Ingredients.IngredientDictionary = item.Ingredients.Ingredient.Zip(item.Ingredients.IngredientAmount, (k, v) => new { Key = k, Value = v })
             .ToDictionary(x => x.Key, x => x.Value);
            }
        }

        private async Task CalcPricesAsync()
        {
            List<Task<int>> Tasklist = Results.Items.Select(item => GetProfit(item)).ToList();
            var results = await Task.WhenAll(Tasklist);

            foreach (var item in Results.Items)
            {
                item.ResultProfit = results[Results.Items.IndexOf(item)];
            }
        }

        private async Task<int> GetProfit(Item Item)
        {
            var resultPriceListTask = _tarkovMarketApiService.FindItem(Item.ResultItemName);
            var IngredientPriceTask = GetIngredientPrice(Item);

            await Task.WhenAll(resultPriceListTask, IngredientPriceTask);

            var resultPriceList = resultPriceListTask.Result;
            var IngredientPrice = IngredientPriceTask.Result;

            if (resultPriceList.Items.Count > 0)
            {
                int resultProfit = Item.ResultCount * resultPriceList.Items[0].Avg24hPrice;
                int NetProfit = resultProfit - IngredientPrice.Sum();
                return NetProfit;
            }
            else return 0;
        }

        private async Task<int> GetSingleIngredientPrice(string Item, int ammount)
        {
            var resultPriceList = await _tarkovMarketApiService.FindItem(Item);
            if (resultPriceList.Items.Count > 0)
            {
                return resultPriceList.Items[0].Avg24hPrice * ammount;
            }
            else return 0;
        }

        private async Task<int[]> GetIngredientPrice(Item Item)
        {
            List<Task<int>> Tasklist = Item.Ingredients.IngredientDictionary.Select(item => GetSingleIngredientPrice(item.Key, item.Value)).ToList();
            return await Task.WhenAll(Tasklist);
        }

        private void MoveItemsToStations()
        {
            foreach (var item in Results.Items)
            {
                switch (item.Station)
                {
                    case Stations.Intel:
                        IntelData.Add(item);
                        break;
                    case Stations.Lav:
                        LavaData.Add(item);
                        break;
                    case Stations.Work:
                        WorkData.Add(item);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async Task CalcPrices()
        {
            IntelData.Clear();
            LavaData.Clear();
            WorkData.Clear();

            SetStatus = "Updating prices...";
            SetButtonStatus = false;
            LoadJson();
            await CalcPricesAsync();
            MoveItemsToStations();
            SetStatus = "Done";
            SetButtonStatus = true;
        }

        public ObservableCollection<Item> IntelDataCollection
        {
            get => IntelData;

            set
            {
                IntelData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> LavDataCollection
        {
            get => LavaData;

            set
            {
                LavaData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> WorkDataCollection
        {
            get => WorkData;

            set
            {
                WorkData = value;
                OnPropertyChanged();
            }
        }

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
