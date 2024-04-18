using Forum.Application.Repositories;
using Forum.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IDomainEntity

    {
        private readonly DatabaseContext _context;

        protected IQueryable<TEntity> Query => _context.Set<TEntity>();

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
          
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(long? id)
        {
            return await _context.Set<TEntity>()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>()
                .Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
