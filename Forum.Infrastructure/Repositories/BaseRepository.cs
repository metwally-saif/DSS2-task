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
            _context.Set<TEntity>().Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is TEntity)
                    {
                        // Using a NoTracking query means we get the entity from the database, but don't track any changes.
                        var databaseEntity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);

                        // Values in the database that have been modified since we loaded the entity into memory.
                        var databaseValues = entry.GetDatabaseValues();

                        if (databaseEntity == null)
                        {
                            // The entity was deleted in the database, so we can't update it.
                            throw new Exception("Unable to update the entity because it was deleted by another user.");
                        }
                        else
                        {
                            // The entity hasn't been deleted, so we can update it.
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                    }
                }

                // Try to save the changes again.
                await _context.SaveChangesAsync();
            }

            return entity;
        }
    }
}
