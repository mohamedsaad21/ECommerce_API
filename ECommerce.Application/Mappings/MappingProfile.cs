using AutoMapper;
using ECommerce.Application.Dtos.Category;
using ECommerce.Application.Dtos.Order;
using ECommerce.Application.Dtos.Product;
using ECommerce.Application.Dtos.ShoppingCart;
using ECommerce.Domain.Entities;
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

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();
        }
    }
}
