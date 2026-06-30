using AutoMapper;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models;

namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DTO a ViewModel
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();

            //ViewModel a DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
        }
    }
}
