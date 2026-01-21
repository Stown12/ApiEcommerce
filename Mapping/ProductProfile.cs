using System;
using AutoMapper;
using webApi.Models;
using webApi.Models.Dtos;

namespace webApi.Mapping;

public class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.Name))
        .ReverseMap();
        CreateMap<Product, CreateProductoDto>().ReverseMap();
        CreateMap<Product, UpdateProductoDto>().ReverseMap();

    }

}
