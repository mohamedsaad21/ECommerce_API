using AutoMapper;
using ECommerce.Application.Dtos.Category;
using ECommerce.Application.Dtos.Order;
using ECommerce.Application.Dtos.Product;
using ECommerce.Application.Dtos.ShoppingCart;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Enums;
namespace ECommerce.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CartUpdateDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();

            CreateMap<ShoppingCartItem, ShoppingCartDTO>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartCreateDTO>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartUpdateDTO>().ReverseMap();

            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();


            CreateMap<OrderToReturnDTO, Order>().ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName));


            CreateMap<PaymentIntentDTO, Order>().ReverseMap()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));
        }
    }
}
