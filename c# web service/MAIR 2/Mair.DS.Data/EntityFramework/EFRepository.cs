using Mair.DS.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Data.EntityFramework
{
    public class EFRepository<T, K> : IRepository<T>
        where T : class, new()
        where K: DbContext
    {
        public K Context { get; set; }

        public EFRepository(K context)
        {
            this.Context = context;
            
        }

        public T Create(T entity)
        { 
            var dbSet = this.Context.Set<T>();

            var ret = dbSet.Add(entity);

            this.Context.SaveChanges();

            return ret.Entity;
        }

        public bool Delete(long id)
        {
            var dbset = this.Context.Set<T>();

            T entity = dbset.Find((long)id);

            var ret = dbset.Remove(entity);

            if (ret.State == EntityState.Deleted)
            {
                this.Context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public void Dispose()
        {
            //TODO: da implementare
            //Context.Dispose();
        }

        public virtual async Task<List<T>> Read()
        {
            var dbSet = this.Context.Set<T>();

            return await dbSet.ToListAsync();
        }

        public List<T> Read(int itemsToSkip, int itemsNumber)
        {
            var dbSet = this.Context.Set<T>();

            return dbSet.ToListAsync().Result.Skip(itemsToSkip).Take(itemsNumber).ToList();
        }

        public T Read(long id)
        {
            var dbSet = this.Context.Set<T>();

            return dbSet.Find(id);
        }

        public bool Exist(long id)
        {
            var dbSet = this.Context.Set<T>();
            if (dbSet.Find(id) == null)
                return false;
            else
                return true;
        }

        public T Update(long id, T entity)
        {
            var dbSet = this.Context.Set<T>();

            var ret = dbSet.Update(entity);

            this.Context.SaveChanges();

            return ret.Entity;
        }
    }
}
