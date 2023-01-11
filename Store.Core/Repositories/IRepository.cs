using System;
using Store.Core.Entities;
using System.Linq.Expressions;

namespace Store.Core.Repositories
{
	public interface IRepository<TEntity>
	{
        Task AddAsync(TEntity entity); 
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp, params string[] includes);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> exp, params string[] includes);
        void Remove(TEntity entity);
        int Commit();

        Task<int> CommitAsync();
    }
}

