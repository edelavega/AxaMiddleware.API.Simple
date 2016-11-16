using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace AXA.Middleware.API
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;

        public GenericRepository(DbContext context)
        {
            this.Context = context;
        }

        public virtual T Get<T>(object id) where T : class
        {
            T obj = Context.Set<T>().Find(id);
            return obj;
        }

        public virtual TChild GetInclude<TParent, TChild>(Func<TParent, bool> where,
            Func<TChild, bool> childWhere, string navigationProperty)
            where TParent : class
            where TChild : class
        {

            var obj = Context.Entry(Context.Set<TParent>().FirstOrDefault(where))
                .Collection(navigationProperty)
                .Query()
                .Cast<TChild>()
                .Where(childWhere)
                .FirstOrDefault();

            return obj;
        }


        public IEnumerable<T> GetAll<T>() where T : class
        {
            return Context.Set<T>();
        }

        public virtual IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public virtual T FindFirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T Add<T>(long sessionId, T entity) where T : class
        {
            Context.Set<T>().Add(entity);
            return entity;
        }

        public virtual T Add<T>(T entity, bool logError) where T : class
        {
            Context.Set<T>().Add(entity);
            return entity;
        }

        public virtual bool AddRange<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().AddRange(entities);
            return true;
        }

        public virtual bool Remove<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
            return true;
        }

        public virtual bool RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().RemoveRange(entities);
            return true;
        }

        public virtual T Update<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    Context.Dispose();

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}