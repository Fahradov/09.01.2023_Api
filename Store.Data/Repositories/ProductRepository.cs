using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;

namespace Store.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(StoreDbContext context) : base(context)
        {
        }
    }
}

