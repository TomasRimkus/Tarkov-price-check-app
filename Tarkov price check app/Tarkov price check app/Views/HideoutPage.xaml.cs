using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Tarkov_price_check_app.Models;
using Tarkov_price_check_app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Tarkov_price_check_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HideoutPage : ContentPage, INotifyPropertyChanged
    {
        public HideoutPage()
        {
            InitializeComponent();
        }
    }
}