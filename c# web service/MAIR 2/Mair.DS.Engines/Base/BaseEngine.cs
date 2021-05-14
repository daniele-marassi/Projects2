using Mair.DS.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Mair.DS.Engines.Base
{
    public class BaseEngine<T> : IDisposable
        where T : class, new()
    {
        #region Fields & Properties

        protected IRepository<T> Repository { get; set; }

        #endregion

        #region Constructors

        public BaseEngine(IRepository<T> repository)
        {
            Repository = repository;
        }

        #endregion

        #region CRUD Methods

        public virtual T Create(T entity)
        {
            var result = Repository.Create(entity);

            return result;
        }

        public virtual List<T> Read()
        {
            var results = Repository.Read().Result;

            return results;
        }

        public virtual List<T> Read(int itemsToSkip, int itemsNumber)
        {
            var results = Repository.Read(itemsToSkip, itemsNumber);

            return results;
        }

        public virtual T Read(int id)
        {
            var result = Repository.Read(id);

            return result;
        }

        public virtual T Update(long id, T entity)
        {
            var result = Repository.Update(id, entity);

            return result;
        }

        public virtual bool Delete(int id)
        {
            var result = Repository.Delete(id);

            return result;
        }

        public virtual void Dispose()
        {
            // to do ...
            //throw new NotImplementedException();
        }

        #endregion
    }
}
