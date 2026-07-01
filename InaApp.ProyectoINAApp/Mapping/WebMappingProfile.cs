using AutoMapper;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Producto;

namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DTO a ViewModel
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();

            //ViewModel a DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>();
        }
    }
}
