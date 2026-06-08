using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using inaApp.Repository;
using inaApp.Services;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Api.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //base datos- dbcontext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer
                (configuration.GetConnectionString("DefaultConnection"))
             );

            //inyecciones de dependencias de servicios
            services.AddScoped<IGenericService<Producto>, ProductoService>();
            services.AddScoped<IGenericService<Cliente>, ClienteService>();


            //inyecciones de dependencias de repositorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();


            return services;
        }
    }
}
