using System;
namespace Store.Core.Entities
{
	public class Product:BaseEntity
	{
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
		public string? Image { get; set; }
		public Category Category { get; set; }
    }
}

