using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Input;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Tarkov_price_check_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarkov_price_check_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PriceCheckPage : ContentPage, INotifyPropertyChanged
    {
        public PriceCheckPage()
        {
            InitializeComponent();
        }
    }

}



