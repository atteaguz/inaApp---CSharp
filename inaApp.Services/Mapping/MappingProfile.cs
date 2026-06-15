using AutoMapper;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DTO a Entidad
            CreateMap<ProductoCreateDTO, Producto>();
            //DTOUpdate a Entidad
            CreateMap<ProductoUpdateDTO, Producto>();
            //Entidad a DTO
            CreateMap<Producto, ProductoResponseDTO>();
        }
    }
}