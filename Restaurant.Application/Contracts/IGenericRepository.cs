using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Contracts
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        IQueryable<T> GetAll();
        Task<T> GetById(int id, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<T> Create(T entity);
        T Update(T entity);
        Task<T> Delete(int id);
        Task<int> SaveChangesAsync();
        Task<Order?> GetPendingOrderWithItemsAsync(string customerId);
        

    }
}
