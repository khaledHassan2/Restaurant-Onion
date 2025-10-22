using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Contracts
{
    public interface IGenericRepository<T> where T:ModelBase
    {
        public Task<IQueryable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(int id);
        public Task<int> SaveChangesAsync();
    }
}
