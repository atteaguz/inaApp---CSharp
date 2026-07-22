using AutoMapper;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Categoria;
using InaApp.ProyectoINAApp.Models.Cliente;
using InaApp.ProyectoINAApp.Models.Producto;
using static inaApp.Common.Enums.Enumeradores;
namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //producto
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();

            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>()
                .ForMember(dest => dest.CategoriaId,
                    opt => opt.MapFrom(src => src.CategoriaId));

            //categoria
            CreateMap<CategoriaResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaEditViewModel>();

            CreateMap<CategoriaIndexViewModel, CategoriaResponseDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaUpdateDTO>();

            //cliente
            // DTO → ViewModel
            CreateMap<ClienteResponseDTO, ClienteIndexViewModel>();
            CreateMap<ClienteResponseDTO, ClienteEditViewModel>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (int)Enum.Parse(typeof(TipoIdentificacionEnum), src.TipoIdentificacion)));

            // ViewModel → DTO
            CreateMap<ClienteIndexViewModel, ClienteResponseDTO>();
            CreateMap<ClienteCreateViewModel, ClienteCreateDTO>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (TipoIdentificacionEnum)src.TipoIdentificacion));

            CreateMap<ClienteEditViewModel, ClienteUpdateDTO>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (TipoIdentificacionEnum)src.TipoIdentificacion));
        }
    }
}
