namespace Eco
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<T> where T : class
    {
        Task<T> DefaultGetById(int id);

        Task<T> DefaultCreate(T entity);

        Task<bool> DefaultUpdate(T entity);

        Task<bool> DefaultDelete(int id);

        Task<bool> DefaultDelete(T entity);

        Task<IEnumerable<T>> DefaultGetAll();

        Task<int> DefaultCreateRange(List<T> list);
    }
}
