using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mair.DS.Data.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T: class, new()
    {
        T Create(T entity);


        Task<List<T>> Read();

        List<T> Read(int itemsToSkip, int itemsNumber);

        T Read(long id);

        bool Exist(long id);

        T Update(long id, T entity);


        bool Delete(long id);

    }
}
