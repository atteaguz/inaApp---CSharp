using AutoMapper;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Categoria;
using InaApp.ProyectoINAApp.Models.Producto;
using InaApp.ProyectoINAApp.Models.Categoria;
namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DTO a ViewModel
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaEditViewModel>();

            //ViewModel a DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();

            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>()
                .ForMember(dest => dest.CategoriaId,
                    opt => opt.MapFrom(src => src.CategoriaId));

            CreateMap<CategoriaIndexViewModel, CategoriaResponseDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaUpdateDTO>();
        }
    }
}
