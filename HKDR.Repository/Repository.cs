using HKDR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HKDR.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ERPDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(ERPDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // Create
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Read
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        // Update
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        // Delete
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task RemoveByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                Remove(entity);
        }

        // Exists / Count
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        // Save
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
