using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Cliente;
using inaApp.Entities;
using inaApp.Repository;
using inaApp.Services;
using Microsoft.EntityFrameworkCore;
using inaApp.Services.Mapping;
using inaApp.DTOs.Categoria;

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

            //inyeccion de AutoMapper
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));


            //inyecciones de dependencias de servicios
            services.AddScoped<IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>, ProductoService>();
            services.AddScoped<IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>, ClienteService>();
            services.AddScoped<IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>, CategoriaService>();


            //inyecciones de dependencias de repositorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();


            return services;
        }
    }
}