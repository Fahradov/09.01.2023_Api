using System;
namespace StoreApi.Client.Dtos.ProductDtos
{
	public class ProductListItemDto
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
        public CategoryInProductListItemDto Category { get; set; }

        public class CategoryInProductListItemDto
        {
            public string Name { get; set; }
        }
    }
}

