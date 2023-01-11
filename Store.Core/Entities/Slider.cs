using System;
namespace Store.Core.Entities
{
	public class Slider:BaseEntity
	{
		public int Order { get; set; }
		public string Image { get; set; }
		public string RedirectUrl { get; set; }
	}
}

