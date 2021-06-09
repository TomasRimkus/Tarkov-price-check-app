using Autofac;
using Tarkov_price_check_app.ViewModels;

namespace Tarkov_price_check_app.Services
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TarkovMarketApiService>().As<ITarkovMarketApiService>();
            builder.RegisterType<HideoutViewModel>().As<IHideoutViewModel>();
            builder.RegisterType<PriceCheckViewModel>().As<IPriceCheckViewModel>();
            return builder.Build();
        }
    }
}
