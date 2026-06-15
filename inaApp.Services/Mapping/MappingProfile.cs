using AutoMapper;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Cliente;
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
            CreateMap<ClienteCreateDTO, Cliente>();

            //DTOUpdate a Entidad
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();

            //Entidad a DTO
            CreateMap<Producto, ProductoResponseDTO>();
            CreateMap<Cliente, ClienteResponseDTO>();
        }
    }
}