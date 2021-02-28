using System.ComponentModel;
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



