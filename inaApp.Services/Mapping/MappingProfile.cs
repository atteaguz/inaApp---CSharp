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
                    opt => opt.MapFrom(src => src.CategoriaId))
                .ForMember(dest => dest.DescuentoMaximo, opt => opt.MapFrom(src => src.DescuentoMaximo));

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

            CreateMap<FacturaCreateDTO, Factura>()
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal))
                .ForMember(dest => dest.Descuento, opt => opt.MapFrom(src => src.Descuento))
                .ForMember(dest => dest.ImpuestoTotal, opt => opt.MapFrom(src => src.ImpuestoTotal))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));

            //FacturaDetalle
            CreateMap<FacturaDetalle, FacturaDetalleResponseDTO>()
                .ForMember(dest => dest.ProductoNombre,
                    opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<FacturaDetalleCreateDTO, FacturaDetalle>();

            //Nota de Credito
            CreateMap<NotaCredito, NotaCreditoResponseDTO>()
                .ForMember(dest => dest.ClienteNombre,
                    opt => opt.MapFrom(src => $"{src.Cliente.Nombre} {src.Cliente.PrimerApellido} {src.Cliente.SegundoApellido ?? ""}".Trim()))
                .ForMember(dest => dest.ClienteCedula,
                    opt => opt.MapFrom(src => src.Cliente.NumeroIdentificacion))
                .ForMember(dest => dest.FacturaOriginalNumero,
                    opt => opt.MapFrom(src => src.FacturaOriginalId.ToString()))
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.NotaCreditoDetalles));

            CreateMap<NotaCreditoDetalle, NotaCreditoDetalleResponseDTO>()
                .ForMember(dest => dest.ProductoNombre,
                    opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.CantidadOriginal,
                    opt => opt.MapFrom(src => src.FacturaDetalleOriginal.Cantidad));
        }
    }
}