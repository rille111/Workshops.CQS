using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Cqs.Infrastructure;

namespace Cqs.Persistence.RuntimeCache
{
    public class CachedRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private readonly string _uniqueCacheKey;
        private List<TEntity> _Items
        {
            get
            {
                var cacheItem = MemoryCache.Default.GetCacheItem(_uniqueCacheKey);
                if (cacheItem != null && cacheItem.Value != null)
                {
                    // Found
                    var itmz = (List<TEntity>)cacheItem.Value;
                    return itmz;
                }

                // Empty, add to Cache
                var newList = new List<TEntity>();
                MemoryCache.Default.Add(_uniqueCacheKey, newList, DateTime.Now.AddMinutes(10));
                return newList;
            }
            set
            {
                MemoryCache.Default[_uniqueCacheKey] = value;
            }
        }
        private static object threadSafeLocker = new object();

        public IQueryable<TEntity> Items
        {
            get { return _Items.AsQueryable(); }
        }

        public CachedRepository()
        {
            this._uniqueCacheKey = typeof(TEntity).FullName;
        }

        public IQueryable<TEntity> ItemsWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return _Items
                .AsQueryable()
                .Where(predicate);
        }

        public TEntity ItemBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _Items
                .AsQueryable()
                .SingleOrDefault(predicate);
        }

        public void Create(TEntity instance)
        {
            lock (threadSafeLocker)
            {
                instance.Id = this._Items.Any() ? this._Items.Max(p => p.Id) + 1 : 1;
                _Items.Add(instance);
                _Items = _Items;

            }
        }

        public void Update(TEntity instance)
        {
            // If same instance then the update is already 'done'
            if (_Items.Contains(instance))
                return;

            

            lock (threadSafeLocker)
            {
                var item = ItemBy(p => p.Id == instance.Id);

                if (item != null)
                    Delete(item);

                _Items.Add(instance);
                _Items = _Items;

            }

        }

        public void Delete(TEntity instance)
        {
            lock (threadSafeLocker)
            {
                _Items.Remove(instance);
                _Items = _Items;
            }
        }
    }
}
