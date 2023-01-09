using System;
using AutoMapper;
using Store.Core.Entities;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Dtos.ProductDtos;

namespace StoreApi.Admin.Profiles
{
	public class AdminMapper:Profile
	{
		public AdminMapper()
		{
			CreateMap<Category, CategoryPostDto>();
			CreateMap<CategoryPostDto, Category>();
			CreateMap<Category, CategoryDetailDto>()
            CreateMap<Category, CategoryListItemDto>();
			CreateMap<Category, CategoryInProductDetailDto>();
			CreateMap<Category, CategoryInProductListItemDto>();

			CreateMap<Product, ProductDetailDto>()
				.ForMember(x => x.DiscountedPrice, f => f.MapFrom(d => d.SalePrice * (100 - d.DiscountPercent) / 100));
			CreateMap<Product, ProductListItemDto>()
				.ForMember(x => x.DiscountedPrice, f => f.MapFrom(d => d.SalePrice * (100 - d.DiscountPercent) / 100));
			CreateMap<Product, ProductPostDto>().ReverseMap();
        }
    }
}

