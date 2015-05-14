using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// Generic repository (Happy path). The sad path would be to use specialized repo's.
    /// </summary>
    /// <example>
    /// Sad path is to make specialized interfaces upon the generic one, ie: public interface IUserRepository : IRepository<User>
    /// </example>
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> Items { get; }
        IQueryable<TEntity> ItemsWhere(Expression<Func<TEntity, bool>> predicate);

        TEntity ItemBy(Expression<Func<TEntity, bool>> predicate);
        void Create(TEntity instance);
        void Update(TEntity instance);
        void Delete(TEntity instance);
    }
}
