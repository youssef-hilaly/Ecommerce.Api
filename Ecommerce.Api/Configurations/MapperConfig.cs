using AutoMapper;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Order;
using Ecommerce.Entities.Dtos.Product;
using Ecommerce.Entities.Dtos.Store;
using Ecommerce.Entities.Dtos.Users;

namespace Ecommerce.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Store, CreateStoreDto>().ReverseMap();
            CreateMap<Store, GetStoreDto>().ReverseMap();
            
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();

            CreateMap<OrderItem, CreateOrderItemDto>().ReverseMap();

        }
    }
}
