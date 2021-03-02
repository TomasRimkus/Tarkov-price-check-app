using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Tarkov_price_check_app.Views;
using Xamarin.Forms;
using MvvmHelpers.Commands;

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

        async Task Refresh()
        {
            CalcPrices();
        }


        public void LoadJson()
        {
            var assembly = typeof(HideoutPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.Contains("data.json"))
                {
                    Stream stream = assembly.GetManifestResourceStream(res);

                    using (StreamReader r = new StreamReader(stream))
                    {
                        string json = r.ReadToEnd();
                        Results = JsonConvert.DeserializeObject<HideoutItems.FullItems>(json);
                    }
                }
            }
        }

        public async System.Threading.Tasks.Task CalcPricesAsync()
        {
            foreach (var item in Results.Items)
            {
                item.ResultProfit = 0;
                int ingredientTotalPrice = 0;

                for (int i = 0; i < item.Ingredients.Ingredient.Count; i++)
                {
                    var priceList = await ApiService.ApiServiceInstance.FindItemAsync(item.Ingredients.Ingredient[i]);
                    ingredientTotalPrice += item.Ingredients.IngredientAmmount[i] * priceList.Items.First().AvgDayPrice;
                }

                var resultPriceList = await ApiService.ApiServiceInstance.FindItemAsync(item.ResultItemName);
                int resultProfit = item.ResultCount * resultPriceList.Items.First().AvgDayPrice;
                item.ResultProfit = resultProfit - ingredientTotalPrice;
            }
        }

        public void MoveItemsToStations()
        {
            foreach (var item in Results.Items)
            {
                switch (item.Station)
                {
                    case "Intel":
                        IntelData.Add(item);
                        break;
                    case "Lav":
                        LavaData.Add(item);
                        break;
                    case "Work":
                        WorkData.Add(item);
                        break;
                }
            }

        }


        public async void CalcPrices()
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
    }
}
