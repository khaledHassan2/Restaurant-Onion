using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.Context;
using Restaurant.Models;
using System;
using System.Linq;
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
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task<T> Delete(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null)
                return null;

            item.IsDeleted = true;
            _dbSet.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        // ✅ غير async لأننا مش محتاجين await هنا
        public IQueryable<T> GetAll()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }

        public async Task<T> GetById(int id, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // ✅ مش async لأن مفيش await
        public T Update(T entity)
        {
            return _dbSet.Update(entity).Entity;
        }
    }
}
