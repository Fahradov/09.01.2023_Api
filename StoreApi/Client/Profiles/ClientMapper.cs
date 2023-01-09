using System;
using AutoMapper;
using Store.Core.Entities;
using StoreApi.Client.Dtos.CategoryDtos;
using StoreApi.Client.Dtos.ProductDtos;
using static StoreApi.Client.Dtos.ProductDtos.ProductDetailDto;
using static StoreApi.Client.Dtos.ProductDtos.ProductListItemDto;

namespace StoreApi.Client.Profiles
{
	public class ClientMapper:Profile
	{
		public ClientMapper()
		{
			CreateMap<Category, CategoryDetailDto>();
			CreateMap<Category, CategoryListItemDto>();
            CreateMap<Category, CategoryInProductDetailDto>();
            CreateMap<Category, CategoryInProductListItemDto>();

            CreateMap<Product, ProductDetailDto>()
				.ForMember(x => x.DiscountedPrice, f => f.MapFrom(d => d.SalePrice * (100 - d.DiscountPercent) / 100))
				.ReverseMap();


            CreateMap<Product, ProductListItemDto>()
				.ForMember(x => x.DiscountedPrice, f => f.MapFrom(d => d.SalePrice * (100 - d.DiscountPercent) / 100))
                .ReverseMap();



        }
    }
}

