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
            builder.RegisterType<NameListHandler>().As<INameListHandler>();
            builder.RegisterType<HideoutViewModel>().AsSelf();
            builder.RegisterType<PriceCheckViewModel>().AsSelf();
            return builder.Build();
        }
    }
}
