using AutoMapper;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
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
            CreateMap<ClienteCreateDTO, Cliente>();
            CreateMap<CategoriaCreateDTO, Categoria>();

            //DTOUpdate a Entidad
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();
            CreateMap<CategoriaUpdateDTO, Categoria>();

            //Entidad a DTO
            CreateMap<Producto, ProductoResponseDTO>()
                .ForMember(dest => dest.CategoriaNombre,
                    opt => opt.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.CategoriaId,
                    opt => opt.MapFrom(src => src.CategoriaId));

            CreateMap<Cliente, ClienteResponseDTO>();
            CreateMap<Categoria, CategoriaResponseDTO>();

            //Factura
            CreateMap<Factura, FacturaResponseDTO>()
                .ForMember(dest => dest.ClienteNombre,
                    opt => opt.MapFrom(src => $"{src.Cliente.Nombre} {src.Cliente.PrimerApellido} {src.Cliente.SegundoApellido ?? ""}".Trim()))
                .ForMember(dest => dest.ClienteCedula,
                    opt => opt.MapFrom(src => src.Cliente.NumeroIdentificacion))
                .ForMember(dest => dest.ClienteTelefono,
                    opt => opt.MapFrom(src => src.Cliente.Telefono ?? "No registrado"))
                .ForMember(dest => dest.ClienteCorreo,
                    opt => opt.MapFrom(src => src.Cliente.CorreoElectronico ?? "No registrado"))
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.FacturaDetalles));

            CreateMap<Factura, FacturaListDTO>()
                .ForMember(dest => dest.ClienteNombre,
                    opt => opt.MapFrom(src => $"{src.Cliente.Nombre} {src.Cliente.PrimerApellido} {src.Cliente.SegundoApellido ?? ""}".Trim()));

            CreateMap<FacturaCreateDTO, Factura>();

            //FacturaDetalle
            CreateMap<FacturaDetalle, FacturaDetalleResponseDTO>()
                .ForMember(dest => dest.ProductoNombre,
                    opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<FacturaDetalleCreateDTO, FacturaDetalle>();
        }
    }
}