using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.Context;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly RestaurantDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(RestaurantDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
          return  (await _context.AddAsync(entity)).Entity;
        }

        public async Task<T> Delete(int id)
        {
            var item = await _context.Set<T>().FindAsync(id);
            if (item == null)
                return null;

            item.IsDeleted = true;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }


        public async Task<IQueryable<T>> GetAll()
        {
            return await Task.FromResult(_dbSet);
        }

        public async Task<T> GetById(int id,Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }


        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            return await Task.FromResult(_context.Update(entity).Entity);
        }
    }
}
