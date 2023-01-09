using System;
using Microsoft.AspNetCore.Identity;

namespace Store.Core.Entities
{
	public class AppUser:IdentityUser
	{
		public string FullName { get; set; }
		public bool IsMember { get; set; }
	}
}

