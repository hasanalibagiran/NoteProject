using System.Linq.Expressions;
using Core.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> 
    where T : BaseEntity, new()
    
    {
        private readonly AppDbContext _context;
    
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
        {

            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter);
            
        }

        public async Task<T> GetByIdAsync(int id)
        {
            
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            
            return await ( filter== null ? _context.Set<T>().AsNoTracking().ToListAsync() : _context.Set<T>().AsNoTracking().Where(filter).ToListAsync() );
            
        }

        public async Task AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            
            
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task AddAllAsync(List<T> entities)
        {
            
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateAll(List<T> entities)
        {
            
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
            
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            
        }

        public void Delete(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.IsActive = false;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            
        }
    }
}