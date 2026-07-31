using AutoMapper;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Categoria;
using InaApp.ProyectoINAApp.Models.Cliente;
using InaApp.ProyectoINAApp.Models.Factura;
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
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>()
                    .ForMember(dest => dest.TipoImpuesto, opt => opt.MapFrom(src => src.TipoImpuesto))
                    .ForMember(dest => dest.PorcentajeImpuesto, opt => opt.MapFrom(src => src.PorcentajeImpuesto))
                    .ForMember(dest => dest.DescuentoMaximo, opt => opt.MapFrom(src => src.DescuentoMaximo))
                    .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio));
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>()
                .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.CategoriaId))
                .ForMember(dest => dest.TipoImpuesto, opt => opt.MapFrom(src => src.TipoImpuesto))
                .ForMember(dest => dest.PorcentajeImpuesto, opt => opt.MapFrom(src => src.PorcentajeImpuesto))
                .ForMember(dest => dest.DescuentoMaximo, opt => opt.MapFrom(src => src.DescuentoMaximo));

            //categoria
            CreateMap<CategoriaResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaEditViewModel>();

            CreateMap<CategoriaIndexViewModel, CategoriaResponseDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaUpdateDTO>();

            //cliente
            //DTO → ViewModel
            CreateMap<ClienteResponseDTO, ClienteIndexViewModel>();
            CreateMap<ClienteResponseDTO, ClienteEditViewModel>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (int)Enum.Parse(typeof(TipoIdentificacionEnum), src.TipoIdentificacion)));

            //viewModel → DTO
            CreateMap<ClienteIndexViewModel, ClienteResponseDTO>();
            CreateMap<ClienteCreateViewModel, ClienteCreateDTO>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (TipoIdentificacionEnum)src.TipoIdentificacion));

            CreateMap<ClienteEditViewModel, ClienteUpdateDTO>()
                .ForMember(dest => dest.TipoIdentificacion,
                    opt => opt.MapFrom(src => (TipoIdentificacionEnum)src.TipoIdentificacion));

            //factura
            CreateMap<FacturaCreateViewModel, FacturaCreateDTO>()
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.Detalles));

            CreateMap<FacturaDetalleViewModel, FacturaDetalleCreateDTO>();

            CreateMap<FacturaResponseDTO, FacturaDetailsViewModel>()
                .ForMember(dest => dest.ClienteTelefono,
                    opt => opt.MapFrom(src => src.ClienteTelefono ?? "No registrado"))
                .ForMember(dest => dest.ClienteCorreo,
                    opt => opt.MapFrom(src => src.ClienteCorreo ?? "No registrado"))
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.Detalles));

            CreateMap<FacturaListDTO, FacturaIndexViewModel>();
            CreateMap<FacturaDetalleResponseDTO, FacturaDetalleViewModel>();

            //nota de credito
            CreateMap<NotaCreditoResponseDTO, NotaCreditoIndexViewModel>()
                .ForMember(dest => dest.FacturaOriginalNumero,
                    opt => opt.MapFrom(src => src.FacturaOriginalId.ToString()))
                .ForMember(dest => dest.CantidadProductos,
                    opt => opt.MapFrom(src => src.Detalles.Count));

            CreateMap<NotaCreditoResponseDTO, NotaCreditoDetailsViewModel>()
                .ForMember(dest => dest.FacturaOriginalNumero,
                    opt => opt.MapFrom(src => src.FacturaOriginalId.ToString()))
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.Detalles));

            CreateMap<NotaCreditoDetalleResponseDTO, NotaCreditoDetalleViewModel>();

            //nota de credito a DTOs
            CreateMap<NotaCreditoCreateViewModel, NotaCreditoCreateDTO>()
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(src => src.Detalles.Where(d => d.Seleccionado && d.CantidadAcreditar > 0)));

            CreateMap<NotaCreditoDetalleCreateViewModel, NotaCreditoDetalleCreateDTO>()
                .ForMember(dest => dest.Cantidad,
                    opt => opt.MapFrom(src => src.CantidadAcreditar));
        }
    }
}
