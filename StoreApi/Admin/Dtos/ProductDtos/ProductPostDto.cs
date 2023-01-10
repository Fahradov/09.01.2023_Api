using System;
using FluentValidation;

namespace StoreApi.Admin.Dtos.ProductDtos
{
	public class ProductPostDto
	{
		public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public IFormFile? ImageFile { get; set; }

    }

    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            RuleFor(x => x.SalePrice).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.CostPrice).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.DiscountPercent).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            RuleFor(x => x.ImageFile)
                .Must(x => x.ContentType == "image/png" || x.ContentType == "image/jpeg").WithMessage("File type must be png,jpg or jpeg")
                .Must(x => x.Length <= 3145728).WithMessage("file size must be lower than 3MB");
        }
    }
}

