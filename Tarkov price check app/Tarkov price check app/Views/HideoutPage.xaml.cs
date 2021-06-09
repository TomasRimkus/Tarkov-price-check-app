using Autofac;
using System.ComponentModel;
using Tarkov_price_check_app.Services;
using Tarkov_price_check_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarkov_price_check_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HideoutPage : ContentPage, INotifyPropertyChanged
    {
        public HideoutPage()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var Service = scope.Resolve<IHideoutViewModel>();
                BindingContext = Service;
            }
        }
    }
}

