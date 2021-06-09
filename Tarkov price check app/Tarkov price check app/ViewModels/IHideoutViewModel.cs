using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using Tarkov_price_check_app.Models;

namespace Tarkov_price_check_app.ViewModels
{
    public interface IHideoutViewModel
    {
        string[] GetStationsList { get; }
        ObservableCollection<HideoutItems.Item> IntelDataCollection { get; set; }
        bool IsIntelVisible { get; set; }
        bool IsLavVisible { get; set; }
        bool IsWorkVisible { get; set; }
        ObservableCollection<HideoutItems.Item> LavDataCollection { get; set; }
        AsyncCommand RefreshCommand { get; }
        bool SetButtonStatus { get; set; }
        int SetCurrentStation { get; set; }
        string SetStatus { get; set; }
        ObservableCollection<HideoutItems.Item> WorkDataCollection { get; set; }
    }
}