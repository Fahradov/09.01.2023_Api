using System;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;

namespace Store.Data.Repositories
{
	public class SliderRepository: Repository<Slider>, ISliderRepository
    {

        public SliderRepository(StoreDbContext context) : base(context)
        {
        }
    }
}

