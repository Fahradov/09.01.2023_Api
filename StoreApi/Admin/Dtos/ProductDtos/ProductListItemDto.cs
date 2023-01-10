using System;
namespace StoreApi.Admin.Dtos.ProductDtos
{
	public class ProductListItemDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImageUrl { get; set; }
        public CategoryInProductListItemDto Category { get; set; }
    }

    public class CategoryInProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

