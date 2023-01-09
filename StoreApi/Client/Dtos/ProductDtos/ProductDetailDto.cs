using System;
namespace StoreApi.Client.Dtos.ProductDtos
{
	public class ProductDetailDto
	{
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
        public CategoryInProductDetailDto Category { get; set; }

        public class CategoryInProductDetailDto
        {
            public string Name { get; set; }
        }
    }
}

