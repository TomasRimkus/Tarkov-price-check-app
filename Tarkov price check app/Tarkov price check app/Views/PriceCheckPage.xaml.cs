using Autofac;
using System.ComponentModel;
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
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope()) { 
                var Service = scope.Resolve<IPriceCheckViewModel>();
                BindingContext = Service;
            }
        }
    }
}



