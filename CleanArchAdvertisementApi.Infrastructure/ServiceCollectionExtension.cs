using CleanArchAdvertisementApi.Application.Interfaces;
using CleanArchAdvertisementApi.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchAdvertisementApi.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();
        }
    }
}
