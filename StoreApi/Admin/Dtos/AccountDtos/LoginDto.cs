using System;
using FluentValidation;

namespace StoreApi.Admin.Dtos.AccountDtos
{
	public class LoginDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class LoginDtoValidator : AbstractValidator<LoginDto>
	{
		public LoginDtoValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().NotNull().MinimumLength(6).MaximumLength(30);
			RuleFor(x => x.Password).NotNull().NotNull().MinimumLength(8).MaximumLength(20); 
		}
	}
}

