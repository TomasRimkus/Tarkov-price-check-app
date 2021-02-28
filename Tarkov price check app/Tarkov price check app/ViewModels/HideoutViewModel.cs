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
        public HideoutItems.Root Results = new HideoutItems.Root();

        public ObservableCollection<HideoutItems.ResultItem> IntelData = new ObservableCollection<HideoutItems.ResultItem>();
        public ObservableCollection<HideoutItems.ResultItem> LavaData = new ObservableCollection<HideoutItems.ResultItem>();
        public ObservableCollection<HideoutItems.ResultItem> WorkData = new ObservableCollection<HideoutItems.ResultItem>();

        public ObservableCollection<HideoutItems.ResultItem> IntelDataTemp = new ObservableCollection<HideoutItems.ResultItem>();
        public ObservableCollection<HideoutItems.ResultItem> LavaDataTemp = new ObservableCollection<HideoutItems.ResultItem>();
        public ObservableCollection<HideoutItems.ResultItem> WorkDataTemp = new ObservableCollection<HideoutItems.ResultItem>();
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
                        Results = JsonConvert.DeserializeObject<HideoutItems.Root>(json);
                    }
                }
            }
        }

        public async System.Threading.Tasks.Task<int> CalcIntelAsync()
        {
            foreach (var item in Results.FullItems.IntelCenter.ResultItem)
            {
                foreach (var item1 in item.Ingredients.Ingredient)
                {
                    var name = item.Ingredients.Ingredient;
                    var count = item.Ingredients.IngredientAmmount;
                    var ingredientCount = name.Count;
                    item.IngredientPrice = 0;
                    var totalIngredientPrice = 0;

                    for (int i = 0; i < ingredientCount; i++)
                    {
                        var priceList = await ApiService.ApiServiceInstance.FindItemAsync(name[i]);
                        totalIngredientPrice = totalIngredientPrice + count[i] * priceList.items.First().price;
                    }

                    item.IngredientPrice = totalIngredientPrice;
                }

                var priceList1 = await ApiService.ApiServiceInstance.FindItemAsync(item.ResultItemName);
                int resultPrice = 0;
                resultPrice = item.ResultCount * priceList1.items.First().price;
                item.ResultPrice = 0;
                item.ResultPrice = resultPrice - item.IngredientPrice;
            }
            return 0;
        }

        public async System.Threading.Tasks.Task<int> CalcLavAsync()
        {
            foreach (var item in Results.FullItems.Lavatory.ResultItem)
            {
                foreach (var item1 in item.Ingredients.Ingredient)
                {
                    var name = item.Ingredients.Ingredient;
                    var count = item.Ingredients.IngredientAmmount;
                    var ingredientCount = name.Count;
                    item.IngredientPrice = 0;
                    var totalIngredientPrice = 0;

                    for (int i = 0; i < ingredientCount; i++)
                    {
                        var priceList = await ApiService.ApiServiceInstance.FindItemAsync(name[i]);
                        totalIngredientPrice = totalIngredientPrice + count[i] * priceList.items.First().price;
                    }

                    item.IngredientPrice = totalIngredientPrice;
                }

                var priceList1 = await ApiService.ApiServiceInstance.FindItemAsync(item.ResultItemName);
                int resultPrice = 0;
                resultPrice = item.ResultCount * priceList1.items.First().price;
                item.ResultPrice = 0;
                item.ResultPrice = resultPrice - item.IngredientPrice;
            }
            return 0;
        }

        public async System.Threading.Tasks.Task<int> CalcWorkAsync()
        {
            foreach (var item in Results.FullItems.Workbench.ResultItem)
            {
                foreach (var item1 in item.Ingredients.Ingredient)
                {
                    var name = item.Ingredients.Ingredient;
                    var count = item.Ingredients.IngredientAmmount;
                    var ingredientCount = name.Count;
                    item.IngredientPrice = 0;
                    var totalIngredientPrice = 0;

                    for (int i = 0; i < ingredientCount; i++)
                    {
                        var priceList = await ApiService.ApiServiceInstance.FindItemAsync(name[i]);
                        totalIngredientPrice = totalIngredientPrice + count[i] * priceList.items.First().price;
                    }

                    item.IngredientPrice = totalIngredientPrice;
                }

                var priceList1 = await ApiService.ApiServiceInstance.FindItemAsync(item.ResultItemName);
                int resultPrice = 0;
                resultPrice = item.ResultCount * priceList1.items.First().price;
                item.ResultPrice = 0;
                item.ResultPrice = resultPrice - item.IngredientPrice;
            }
            return 0;
        }

        public async void CalcPrices()
        {
            IntelDataTemp.Clear();
            IntelData.Clear();
            LavaDataTemp.Clear();
            LavaData.Clear();
            WorkDataTemp.Clear();
            WorkData.Clear();

            SetStatus = "Updating prices...";
            LoadJson();
            await CalcIntelAsync();
            await CalcLavAsync();
            await CalcWorkAsync();

            foreach (var data in Results.FullItems.IntelCenter.ResultItem)
            {
                IntelDataTemp.Add(data);
            }
            foreach (var data in Results.FullItems.Lavatory.ResultItem)
            {
                LavaDataTemp.Add(data);
            }
            foreach (var data in Results.FullItems.Workbench.ResultItem)
            {
                WorkDataTemp.Add(data);
            }
            SetStatus = "Done";
            IntelDataCollection = IntelDataTemp;
            LavDataCollection = LavaDataTemp;
            WorkDataCollection = WorkDataTemp;
        }


        public ICommand UpdateHideout
        {
            get;
        }

        public ObservableCollection<HideoutItems.ResultItem> IntelDataCollection
        {
            get { return IntelData; }

            set
            {
                IntelData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HideoutItems.ResultItem> LavDataCollection
        {
            get { return LavaData; }

            set
            {
                LavaData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HideoutItems.ResultItem> WorkDataCollection
        {
            get { return WorkData; }

            set
            {
                WorkData = value;
                OnPropertyChanged();
            }
        }

        public string Status = "Ready";

        public string SetStatus
        {
            get { return Status; }
            set
            {
                Status = value;
                OnPropertyChanged();
            }
        }






    }


}
