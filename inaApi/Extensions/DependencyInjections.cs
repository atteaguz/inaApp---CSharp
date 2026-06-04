using inaApp.Common.interfaces;
using inaApp.Repository;
using inaApp.Services;

namespace inaApp.Api.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //base datos- dbcontext

            //inyecciones de dependencias de servicios
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IClienteService, ClienteService>();


            //inyecciones de dependencias de repositorios
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();


            return services;
        }
    }
}
